import os
import subprocess
from pathlib import Path
import sys
import shutil

ROOT = Path(__file__).parent.resolve()

PROJECTS = {
    "API Server (.NET)": ROOT / "DeniMetrics.WebAPI",
    "React Client (my-react-app)": ROOT / "my-react-app",
    "MAUI Mobile App": ROOT / "DineMetrics.Mobile",
    "Customer Sensor Emulator (WPF)": ROOT / "DineMetrics.CustomerSensorEmulator",
    "Temperature Sensor Emulator (WPF)": ROOT / "DineMetrics.TemperatureSensorEmulator",
}

DOTNET = shutil.which("dotnet")
NPM = shutil.which("npm") or r"C:\Program Files\nodejs\npm.cmd"

def run(cmd, cwd=None):
    print(f"\n▶ Running: {' '.join(cmd)} in {cwd or os.getcwd()}")
    try:
        subprocess.run(cmd, cwd=cwd, check=True)
    except FileNotFoundError as fnf:
        print(f"❌ File not found: {cmd[0]}. Is it installed and in PATH?")
        raise fnf
    except subprocess.CalledProcessError as e:
        print(f"❌ Command failed: {e}")
        raise e

def check_prerequisites():
    print("🔍 Checking prerequisites...")
    if not DOTNET:
        print("❌ .NET SDK is not installed or not in PATH.")
        sys.exit(1)
    else:
        print(f"✅ Found .NET: {DOTNET}")

    if not NPM or not os.path.exists(NPM):
        print("❌ npm is not installed or path is incorrect.")
        sys.exit(1)
    else:
        print(f"✅ Found npm: {NPM}")

def install_dotnet_dependencies():
    print("📦 Restoring .NET projects...")
    for name, path in PROJECTS.items():
        if "react" not in name.lower():
            print(f"🔄 Restoring {name}")
            run([DOTNET, "restore"], cwd=path)

def install_npm_dependencies():
    print("📦 Installing npm dependencies for React...")
    run([NPM, "install"], cwd=PROJECTS["React Client (my-react-app)"])

def build_all():
    print("🛠 Building .NET projects...")
    for name, path in PROJECTS.items():
        if "react" not in name.lower():
            print(f"🔨 Building {name}")
            run([DOTNET, "build", "--configuration", "Release"], cwd=path)

    print("🌐 Building React client...")
    run([NPM, "run", "build"], cwd=PROJECTS["React Client (my-react-app)"])

def run_interactive():
    while True:
        print("\n📦 Select a project to run:")
        for i, name in enumerate(PROJECTS.keys(), 1):
            print(f"{i}. {name}")
        print(f"{len(PROJECTS)+1}. Exit")

        choice = input("Choice: ").strip()
        try:
            choice = int(choice)
            if 1 <= choice <= len(PROJECTS):
                name = list(PROJECTS.keys())[choice - 1]
                path = PROJECTS[name]
                if "react" in name.lower():
                    run([NPM, "start"], cwd=path)
                else:
                    run([DOTNET, "run"], cwd=path)
            elif choice == len(PROJECTS) + 1:
                print("✅ Exiting.")
                break
            else:
                print("❌ Invalid option.")
        except Exception as e:
            print(f"⚠️ Error: {e}")

def main():
    print("🚀 DineMetrics Setup Script")

    check_prerequisites()

    try:
        install_dotnet_dependencies()
        install_npm_dependencies()
        build_all()
        run_interactive()
    except Exception as e:
        print(f"❌ Setup failed: {e}")
        print("💡 Tip: Make sure all required SDKs are installed and NuGet/npm sources are available.")

if __name__ == "__main__":
    main()
