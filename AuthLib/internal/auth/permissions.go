package auth

import (
	"fmt"
	"strings"
)

func GetAllPermissionsString(userName string) string {
	user, exists := users[userName]

	if !exists {
		return ""
	}

	var parts []string
	for item, status := range user.Permissions {
		parts = append(parts, fmt.Sprintf("%s:%d", item, status))
	}
	return strings.Join(parts, ",")
}
