# AltV Tools

A simple .NET tool to download [AltV](https://altv.mp/#/downloads) server binaries, data files and modules.

Supported features ✅:
* Automatically Downloading server binary files, data files and module files
* Ability to choose channel and OS for which to download files
* Only downloads missing/updated files

Unsupported features ⛔:
* Downloading AltV Voice Server files
* Downloading sample resource
* Sample config file

Warning ⚠️:
This is only meant for my own use, I do not promise to ever include any missing features or keep this up to date. However, if you have any questions or susgestions feel free to create issue or pr or even contact me on discord `rolandas`

---

### Usage
```shell 
.\ServerFileDownloader.exe --platform x64_win32 --channel dev --server --data --modules coreclr-module
```
* --platform specifies OS (x64_win32 or x64_linux)
* --channel specifies which release version you want to use (dev, rc or release)
* use --server flag to download/update server binary files
* use --data flag to download/update data files
* --modules flag specifies which modules you want to download, for example `--modules coreclr-module js-module` would include C# module and JS module