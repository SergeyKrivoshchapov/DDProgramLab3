package models

type User struct {
	Name        string
	Password    string
	Permissions map[string]int // value -- status in 0/1/2
}
