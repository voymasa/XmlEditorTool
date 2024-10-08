# XmlEditorTool

## Voymasa's Xml Editor Tool for Sketch Engine v1.0.0
#### Release Date: 7/8/2022

### Overview:
This tool works to edit xml files for Ryan Vandendyck's SketchEngine. You can load multiple xml data files and edit them in their own windows, at the same
time. There is some initial setup required to ensure that the tool works as intended.


### Setup:
When you first run the XmlEditorTool.exe, you will need to configure your settings in the application. 
Click on Application > Settings to open the settings window.
The Top-Level directory should be set to the root directory that contains your project, the RpgBase, and SketchEngine, directories.
The Component to Source Mapper File is a .csv file containing the Component name (that the xml element is for) and the corresponding
header file that contains the definition of that pipeline macro. An example file is contained in the Resources directory of this tool.
The Macro Data Filename is a .csv file that contains the definitions of the pipeline macros from the engine, and how the macro signatures
will be parsed for dispaly and editing. An example file is contained in the Resources directory of this tool, and should contain all of the current
pipeline macro signatures as of the release of this version of the tool. Double-checking the file is warranted.
The Macro Prefix is the string of characters that start the name of the macros in the header file. Currently this is PIPELINE_ with the underscore included.

When the application starts for the first time, it will attempt to find the directories for the settings, but should be double-checked and adjusted for your
project structure. Once your configurations are made, click the Apply button to save your settings. You can then return to the main window by selecting
Settings > Return to Main Window.


### Opening a data file:
An .xml file may be opened in any of the following ways:
1. Select Application > Open...  and browse to the file.
2. Click on the icon in the window pane and browse to the file.
3. Drag an .xml file onto the icon and drop it.

Note that the data file **must** exist within a directory inside of your Top-Level directory, or the tool will crash.


### Editing an .xml data file:
When you open your data file, a new window pane opens with a heirarchy view of the xml elements and attributes, in the left pane.
To begin editing a component, click on the element within the left window pane. The tool will search for the corresponding header file
identified within the Component to Source mapper file, within the folder structure beneath the Top-Level directory, and then parse based upon
the information identified within the Macro Data file. Once the tool finds the macro, it will display each editable attribute in the right window
pane.
Grey values are the default value from the macro.
Green values are non-default values found within the element being edited.
Red values are unsaved changes.
Apply Changes will save the changes to the selected element only.
Expand All Properties will expand all of the component properties in the right window pane.
Expand Modified Properties will collapse all component properties that have the default value, and expand all those that don't have the default value.
Collapse All Properties will collapse all component properties in the right window pane.

Note: don't forget to Apply Changes before attempting to save your data file.


### Saving the .xml data file:
The file can be saved in a couple of ways, in the Actions menu at the top:
1. Select Save XML to save the changes to the .xml file you opened.
2. Select Export to XML...  to save the changes to a new file.

If you select Actions > Close Window, it will close the corresponding window, but leave the remaining windows open.



### Known Issues:
- If you open an .xml file from a location other than within your Top-Level Directory downwards, and then select an element to edit, the tool crashes.
