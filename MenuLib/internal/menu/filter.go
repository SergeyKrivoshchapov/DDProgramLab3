package menu

import (
	"MenuLib/internal/models"
	"strconv"
	"strings"
)

var filteredTree []*models.MenuItem

func FilterByPermissions(permsString string) {
	perms := parsePermissions(permsString)
	filteredTree = filterItems(menuTree, perms)
}

func parsePermissions(permsString string) map[string]int {
	perms := make(map[string]int)
	if permsString == "" {
		return perms
	}

	parts := strings.Split(permsString, ",")
	for _, part := range parts {
		kv := strings.SplitN(part, ":", 2)
		if len(kv) == 2 {
			status, _ := strconv.Atoi(kv[1])
			perms[kv[0]] = status
		}
	}
	return perms
}

func filterItems(items []*models.MenuItem, perms map[string]int) []*models.MenuItem {
	result := []*models.MenuItem{}
	for _, item := range items {
		status := 0
		if val, exists := perms[item.Name]; exists {
			status = val
		}
		if status == 2 {
			continue
		}

		newItem := &models.MenuItem{
			Name:     item.Name,
			Method:   item.Method,
			Status:   status,
			Children: filterItems(item.Children, perms),
		}
		result = append(result, newItem)
	}
	return result
}

func GetFilteredTree() []*models.MenuItem {
	if filteredTree != nil {
		return filteredTree
	}
	return menuTree
}
