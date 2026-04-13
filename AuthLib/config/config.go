package config

type config struct {
	DefaultPointStatus int
}

var App = &config{
	DefaultPointStatus: 1,
}
