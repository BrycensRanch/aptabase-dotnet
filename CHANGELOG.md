## 0.2.0

* Add `Aptabase.Core` for generic .NET support (Does not support crash reporter)
* Use [XDG.Directories](https://www.nuget.org/packages/Xdg.Directories) Cache Directory across all .NET SDKs (This was a requirement for Aptabase.Core) 

## 0.1.0

* Add `EnablePersistence` to persist events on disk before sending them to the server
* Add `EnableCrashReporting` to log application crashes, unhandled exceptions

## 0.0.9

* Fix net8 compatibility issues
* Use `System.Threading.Channels` to send events asynchronously
* Add `IsDebugMode` to `AptabaseOptions` (fixes Android detection + avoids reflection if specified by the consumer of the sdk)
* Add `DeviceModel` to system properties

## 0.0.8

* Use new session id format

## 0.0.7

* Use a more reliable method for Debug/Release mode detection
* Update docs

## 0.0.6

* Fix automatic Debug/Release mode detection

## 0.0.5

* Added support for automatic segregation of Debug/Release events
* Explicit nullable types