#include <iostream>
#include <Windows.h>
#include <vector>
#include <thread>
#include <cstring>
#include <string>

using namespace std;

const int COUNT = 5;
HANDLE hMonitorMutex;
bool forksState[COUNT]; // 0, 1
//int philosPriority[COUNT]; // 0 - low, N - high
HANDLE philosophers[COUNT];
HANDLE hFileMap;
HANDLE hChangeStateEvent;

const wstring MUTEX_NAME = L"StateMutex";
const wstring FILE_NAME = L"StateFileMap";
const wstring EVENT_NAME = L"ChangeStateEvent";
const wstring PHILO_EXE_PATH = L"D:\\PhilosProcess\\x64\\Debug\\PhilosProcess.exe";


HANDLE Create(wstring path, wstring args)
{
	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));

	CreateProcess(&path[0], &args[0], NULL, NULL, TRUE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &pi);
	return pi.hProcess;
}

void Run()
{
	hMonitorMutex = CreateMutex(NULL, FALSE, MUTEX_NAME.c_str());
	hChangeStateEvent = CreateEvent(NULL, TRUE, TRUE, EVENT_NAME.c_str());
	hFileMap = CreateFileMapping(NULL, NULL, PAGE_READWRITE, 0, sizeof(bool[COUNT]), FILE_NAME.c_str());
	
	LPVOID viewPtr = MapViewOfFile(hFileMap, FILE_ALL_ACCESS, 0, 0, 0);
	bool* view = (bool*)viewPtr;
	
	// ініціалізувати масив станів
	view = new bool[COUNT];
	for (int i = 0; i < COUNT; i++)
		view[i] = false;
	// запустити процеси філософи
	for (int i = 0; i < COUNT; i++)
	{
		wstring args = FILE_NAME + L" "
			+ MUTEX_NAME + L" "
			+ to_wstring(i) + L" "
			+ to_wstring(COUNT) + L" "
			+ EVENT_NAME;
		philosophers[i] = Create(PHILO_EXE_PATH, args);
	}
}

void CloseAll()
{
	for (int i = 0; i < COUNT; i++)
		CloseHandle(philosophers[i]);
	CloseHandle(hFileMap);
	CloseHandle(hMonitorMutex);
}

int main(){
	Run();
	WaitForMultipleObjects(COUNT, philosophers, TRUE, INFINITE);
	CloseAll();
}