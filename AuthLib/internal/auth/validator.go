package auth

func AuthenticateUser(name, password string) bool {
	user, exists := users[name]
	if !exists {
		return false
	}
	return user.Password == password
}

func UserExists(name string) bool {
	_, exists := users[name]
	return exists
}
