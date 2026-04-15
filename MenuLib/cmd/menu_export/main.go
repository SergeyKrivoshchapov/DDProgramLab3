package main

/*
#include <stdlib.h>

typedef struct {
	char* Name;
	char* Method;
	int Status;
	int ChildrenCount;
} MenuItemStruct;
*/
import "C"

import (
	"MenuLib/internal/menu"
	"unsafe"
)

//export LoadMenu
func LoadMenu(fileName *C.char) C.int {
	entries, err := menu.LoadMenuFromFile(C.GoString(fileName))
	if err != nil {
		return -1
	}

	menu.BuildTree(entries)
	return 0
}

//export FilterByPermissions
func FilterByPermissions(permsString *C.char) {
	menu.FilterByPermissions(C.GoString(permsString))
}

//export GetRootCount
func GetRootCount() C.int {
	return C.int(menu.GetRootCount())
}

//export GetMenuItem
func GetMenuItem(path *C.char) C.MenuItemStruct {
	item := menu.GetItemByPath(C.GoString(path))

	if item == nil {
		return C.MenuItemStruct{
			Name:          nil,
			Method:        nil,
			Status:        0,
			ChildrenCount: 0,
		}
	}
	return C.MenuItemStruct{
		Name:          C.CString(item.Name),
		Method:        C.CString(item.Method),
		Status:        C.int(item.Status),
		ChildrenCount: C.int(len(item.Children)),
	}
}

//export FreeMenuItem
func FreeMenuItem(item C.MenuItemStruct) {
	if item.Name != nil {
		C.free(unsafe.Pointer(item.Name))
	}
	if item.Method != nil {
		C.free(unsafe.Pointer(item.Method))
	}
}

//export FreeString
func FreeString(str *C.char) {
	if str != nil {
		C.free(unsafe.Pointer(str))
	}
}

func main() {}