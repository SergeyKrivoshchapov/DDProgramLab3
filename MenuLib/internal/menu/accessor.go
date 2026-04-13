package menu

import (
	"MenuLib/internal/models"
	"strconv"
	"strings"
)

func GetRootCount() int {
	tree := GetFilteredTree()
	return len(tree)
}

func GetItemByPath(path string) *models.MenuItem {
	if path == "" {
		return nil
	}
	tree := GetFilteredTree()
	indices := strings.Split(path, "/")

	var current []*models.MenuItem = tree
	var item *models.MenuItem

	for i, idxStr := range indices {
		idx, err := strconv.Atoi(idxStr)
		if err != nil || idx < 0 || idx >= len(current) {
			return nil
		}
		item = current[idx]
		if i < len(indices)-1 {
			current = item.Children
		}
	}
	return item
}

func FindMethodByName(name string) string {
	var search func([]*models.MenuItem) string
	search = func(items []*models.MenuItem) string {
		for _, item := range items {
			if item.Name == name {
				return item.Method
			}
			if method := search(item.Children); method != "" {
				return method
			}
		}
		return ""
	}
	return search(GetFilteredTree())
}
