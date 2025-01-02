
# EA Add-In: Auto Diagram Title Generator

This is an Enterprise Architect (EA) add-in that dynamically manages and updates titles for diagrams. It includes features for auto-generating, modifying, and cleaning up title elements within EA diagrams.

## Features

- **Auto Title Generation**: Automatically adds a title element to diagrams, displaying the diagram name and the last modification date.
- **Dynamic Updates**: Updates title information when diagrams are modified.
- **Duplicate Cleanup**: Removes duplicate title elements during diagram updates or deletion.
- **Event Handling**:
  - Responds to diagram modifications, additions, and deletions.
  - Ensures titles are accurately maintained across operations.

## How It Works

- The add-in listens for specific EA events such as:
  - **`EA_OnNotifyContextItemModified`**: Updates or creates a title element when a diagram is modified.
  - **`EA_OnPostNewDiagram`**: Refreshes the newly created diagram.
  - **`EA_OnPreDeleteDiagram`**: Cleans up title elements associated with the diagram before deletion.

- A "Text" type element is used to display the title and additional metadata, such as the last modification date.

## Installation
![image](https://github.com/user-attachments/assets/12a30c16-1c05-4cbe-92ca-22adf64de145)

### Option 1: Build from Source and Register Manually

1. **Build the Add-In**:
   - Use a .NET development environment, such as Visual Studio, to compile the provided code into a DLL.

2. **Register the DLL**:
   - Open a command prompt as Administrator and register the DLL using `regasm`:
     ```bash
     regasm /codebase YourDllName.dll
     ```

3. **Configure EA**:
   - Open EA.
   - Go to **Extensions > Manage Add-Ins** and ensure the add-in is listed and enabled.

4. **Restart EA**:
   - Restart Enterprise Architect to load the add-in.

### Option 2: Install via MSI File

1. **Obtain the MSI Installer**:
   - Download the MSI file from the repository or the distribution source.

2. **Run the Installer**:
   - Double-click the MSI file to launch the installation wizard.
   - Follow the on-screen instructions to complete the installation.

3. **Verify Installation**:
   - Open EA and check the **Extensions > Manage Add-Ins** menu to ensure the add-in is listed and enabled.

4. **Restart EA**:
   - Restart Enterprise Architect to activate the add-in.

## Usage

1. Open Enterprise Architect and load a project.
2. Perform operations such as:
   - Modifying diagrams.
   - Adding new diagrams.
   - Deleting diagrams.
3. Observe how the title element is dynamically updated or cleaned up.

## Requirements

- **Enterprise Architect**: Ensure you have a valid installation.
- **.NET Framework**: Compatible with the framework version used for building the DLL.
- **Development Environment**: Visual Studio (or similar) for building the add-in (if not using the MSI installer).

## Contribution

Feel free to contribute enhancements or report issues:
- Fork the repository.
- Create a branch for your feature or bugfix.
- Submit a pull request.

## License

This add-in is open source and distributed under the MIT License.

## Contact

For questions or support, reach out to the author or create an issue in the repository.
