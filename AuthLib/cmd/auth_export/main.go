package main

/*
#include <stdlib.h>

*/
import "C"
import (
	"AuthLib/internal/auth"
	"unsafe"
)

//export LoadUsers
func LoadUsers(fileName *C.char) C.int {
	count, err := auth.LoadUsersFromFile(C.GoString(fileName))
	if err != nil {
		return -1
	}

	return C.int(count)
}

//export Authenticate
func Authenticate(name, password *C.char) C.int {
	if auth.AuthenticateUser(C.GoString(name), C.GoString(password)) {
		return 0
	}
	return -1
}

//export GetAllPermissions
func GetAllPermissions(userName *C.char) *C.char {
	goUserName := C.GoString(userName)
	if !auth.UserExists(goUserName) {
		return nil
	}

	perms := auth.GetAllPermissionsString(goUserName)

	return C.CString(perms)
}

//export FreePermissions
func FreePermissions(p *C.char) {
	if p != nil {
		C.free(unsafe.Pointer(p))
	}
}

func main() {}
