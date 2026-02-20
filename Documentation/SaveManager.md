## Class: [SaveManager](./SaveManager.md)

### Description
A plug-and-play script for Unity session and data saving.

### Namespace
[MagmaLabs.Economy](./Economy.md)


### Properties

#### [instance]
- **Type:** static SaveManager
- **Description:** The singleton instance of SaveManager
- **Access:** { get; private set; }

#### [SaveData]
- **Type:** saveData
- **Description:** The up to date data of the current save
- **Access:** { get; private set; }


### Methods

#### [SaveString]
- **Signature:** public void SaveString(string key, string value)
- **Description:** Saves a string value to the save data
- **Parameters:**
  - [key]: The retrieval key 
  - [value]: The value of the string to save
- **Return Type:** void
- **Example Usage:**
  ```csharp
  SaveManager.instance.SaveString("username", "bffwouj3472");

  string username = SaveManager.instance.LoadString("username")//"bffwouj3472"
  ```

#### [SaveFloat]
- **Signature:** public void SaveFloat(string key, float value)
- **Description:** Saves a float value to the save data
- **Parameters:**
  - [key]: The retrieval key
  - [value]: The float value to save
- **Return Type:** void
- **Example Usage:**
  ```csharp
  SaveManager.instance.SaveFloat("volume", 0.75f);

  float volume = SaveManager.instance.LoadFloat("volume")//0.75f
  ```

#### [SaveInt]
- **Signature:** public void SaveInt(string key, int value)
- **Description:** Saves an integer value to the save data
- **Parameters:**
  - [key]: The retrieval key
  - [value]: The integer value to save
- **Return Type:** void
- **Example Usage:**
  ```csharp
  SaveManager.instance.SaveInt("level", 5);

  int level = SaveManager.instance.LoadInt("level")//5
  ```

#### [SaveBool]
- **Signature:** public void SaveBool(string key, bool value)
- **Description:** Saves a boolean value to the save data
- **Parameters:**
  - [key]: The retrieval key
  - [value]: The boolean value to save
- **Return Type:** void
- **Example Usage:**
  ```csharp
  SaveManager.instance.SaveBool("tutorialComplete", true);

  string tutorialComplete = SaveManager.instance.LoadBool("tutorialComplete")//true
  ```

#### [LoadString]
- **Signature:** public string LoadString(string key, string defaultValue = "")
- **Description:** Loads a string value from the save data, returning a default if not found
- **Parameters:**
  - [key]: The retrieval key
  - [defaultValue]: The value to return if the key is not found (default: empty string)
- **Return Type:** string
- **Example Usage:**
  ```csharp
  string username = SaveManager.instance.LoadString("username", "Player");
  ```

#### [LoadFloat]
- **Signature:** public float LoadFloat(string key, float defaultValue = 0f)
- **Description:** Loads a float value from the save data, returning a default if not found
- **Parameters:**
  - [key]: The retrieval key
  - [defaultValue]: The value to return if the key is not found (default: 0f)
- **Return Type:** float
- **Example Usage:**
  ```csharp
  float volume = SaveManager.instance.LoadFloat("volume", 0.5f);
  ```

#### [LoadInt]
- **Signature:** public int LoadInt(string key, int defaultValue = 0)
- **Description:** Loads an integer value from the save data, returning a default if not found
- **Parameters:**
  - [key]: The retrieval key
  - [defaultValue]: The value to return if the key is not found (default: 0)
- **Return Type:** int
- **Example Usage:**
  ```csharp
  int level = SaveManager.instance.LoadInt("level", 1);
  ```

#### [LoadBool]
- **Signature:** public bool LoadBool(string key, bool defaultValue = false)
- **Description:** Loads a boolean value from the save data, returning a default if not found
- **Parameters:**
  - [key]: The retrieval key
  - [defaultValue]: The value to return if the key is not found (default: false)
- **Return Type:** bool
- **Example Usage:**
  ```csharp
  bool tutorialDone = SaveManager.instance.LoadBool("tutorialComplete", false);
  ```

#### [Save]
- **Signature:** public void Save()
- **Description:** Manually saves all current data to the save file. This is called automatically if autoSave is enabled, and when the application quits.
- **Parameters:** None
- **Return Type:** void
- **Example Usage:**
  ```csharp
  SaveManager.instance.Save();
  ```




### Fields

#### [fileName]
- **Type:** string
- **Description:** The name of the save file
- **Access:** protected
- **Default Value:** saveData.json

#### [autoSave]
- **Type:** bool
- **Description:** Whether or not the game should automatically save
- **Access:** protected
- **Default Value:** true

#### [autoSaveInterval]
- **Type:** float
- **Description:** How often to automatically save in seconds, if enabled.
- **Access:** protected
- **Default Value:** 60f

#### [logDebugMessages]
- **Type:** bool
- **Description:** Whether or not to log debug messages
- **Access:** protected
- **Default Value:** false

#### [filePath]
- **Type:** string
- **Description:** The file path of the save file
- **Access:** private
- **Default Value:** System.IO.Path.Combine(Application.persistentDataPath, fileName);


### Usage Notes
1. Add an instance of SaveManager to your scene



### See Also
[SaveData](./SaveData.md)
[MagmaLabs.Economy](./Economy.md)

