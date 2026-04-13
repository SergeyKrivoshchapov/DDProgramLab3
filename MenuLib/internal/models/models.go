package models

type MenuItem struct {
	Name     string
	Method   string
	Status   int
	Children []*MenuItem
}

type RawMenuEntry struct {
	Level  int
	Name   string
	Method string
}
