package menu

import (
	"MenuLib/internal/models"
	"bufio"
	"os"
	"strconv"
	"strings"
)

func LoadMenuFromFile(fileName string) ([]models.RawMenuEntry, error) {
	var entries []models.RawMenuEntry

	file, err := os.Open(fileName)
	if err != nil {
		return nil, err
	}

	defer file.Close()

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := strings.TrimSpace(scanner.Text())
		if line == "" {
			continue
		}
		parts := strings.SplitN(line, " ", 3)
		if len(parts) < 2 {
			continue
		}

		level, _ := strconv.Atoi(parts[0])
		name := parts[1]
		method := ""
		if len(parts) == 3 {
			method = parts[2]
		}

		entries = append(entries, models.RawMenuEntry{
			Level:  level,
			Name:   name,
			Method: method,
		})
	}
	return entries, scanner.Err()
}
