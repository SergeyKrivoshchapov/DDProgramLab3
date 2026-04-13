package menu

import "MenuLib/internal/models"

var menuTree []*models.MenuItem

func BuildTree(entries []models.RawMenuEntry) {
	menuTree = []*models.MenuItem{}
	stack := []*models.MenuItem{}

	for _, entry := range entries {
		item := &models.MenuItem{
			Name:     entry.Name,
			Method:   entry.Method,
			Status:   0,
			Children: []*models.MenuItem{},
		}

		for len(stack) > entry.Level {
			stack = stack[:len(stack)-1]
		}
		if entry.Level == 0 {
			menuTree = append(menuTree, item)
		} else if len(stack) >= entry.Level {
			parent := stack[entry.Level-1]
			parent.Children = append(parent.Children, item)
		}

		if len(stack) <= entry.Level {
			stack = append(stack, item)
		} else {
			stack[entry.Level] = item
		}
	}
}

func GetTree() []*models.MenuItem {
	return menuTree
}
