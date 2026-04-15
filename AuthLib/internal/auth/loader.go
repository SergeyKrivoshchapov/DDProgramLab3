package auth

import (
	"AuthLib/internal/models"
	"bufio"
	"os"
	"strconv"
	"strings"
)

var users map[string]*models.User

func LoadUsersFromFile(fileName string) (int, error) {
	users = make(map[string]*models.User)

	file, err := os.Open(fileName)
	if err != nil {
		return 0, err
	}
	defer file.Close()

	var currentUser *models.User
	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		line := strings.TrimSpace(scanner.Text())
		if line == "" {
			continue
		}

		if strings.HasPrefix(line, "#") {
			// #Name Password
			line = line[1:]
			parts := strings.SplitN(line, " ", 2)
			if len(parts) == 2 {
				currentUser = &models.User{
					Name:        parts[0],
					Password:    parts[1],
					Permissions: make(map[string]int),
				}
				users[currentUser.Name] = currentUser
			}
		} else if currentUser != nil {
			// PName Status
			parts := strings.SplitN(line, " ", 2)
			if len(parts) == 2 {
				status, _ := strconv.Atoi(parts[1])
				currentUser.Permissions[parts[0]] = status
			}
		}
	}

	return len(users), scanner.Err()
}

func GetUsers() map[string]*models.User {
	return users
}
