default: gui

build:
    @dotnet build

tui: build
    @dotnet run --project PasswordManagerApp.TUI
    
gui: build
    @dotnet run --project PasswordManagerApp.Avalonia

