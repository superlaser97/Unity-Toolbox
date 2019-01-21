# Unity-Toolbox
Documentation on what and how to use these tools

## Included Kits ( Big ones )
- SimpleUIDialog
- SimpleUIToast
- AsyncSceneLoader
- DebugTools
  - FPS Counter
  - Console
- Versioning
- Feature Flags


___

## Requirements: 
-	Unity 2018.1 and above
-	Half a brain

___

![alt text](https://github.com/superlaser97/Unity-Toolbox/blob/master/Assets/SUPERLASER/SimpleUIPrompt/README_Header.PNG "SimpleUIDialog")
## SimpleUIDialog
Easy way to show beautifully designed dialog with deletaged buttons. Supports multiple dialog calls at the same time

### Notes
You do not need to any prefab on the scene, it will be auto loaded by the script itself and assigned to do not be destroyed.

### Functions
`SimpleUIDialog.ShowDialog()`

Parameters
-	Title
-	Content
-	Buttons (Delegates and Button Text) ( Optional )
-	Show “X” Button ( Optional )
-	Highlighted Button ( Optional )
After any button is clicked, the dialog will automatically close

`SimpleUIDialog.Dispose()`

Destroys SimpleUIDialog from scene, free-ing up memory. Can be reinstantated by calling any static methods

Additional Settings is on the Prefab at
> Assets/Resource/SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas
-	Accent Color (Highlight color)
-	Animation Spd

* To change the scaling of the UI, Modify the Scale Factor on the Canvas Scaler on the Prefab

___

![alt text](https://github.com/superlaser97/Unity-Toolbox/blob/master/Assets/SUPERLASER/SimpleUIToast/README_Header.PNG "SimpleUIToast")
## SimpleUIToast
Easy way to show Toast with nice animations. Supports multiple toast at the same time

### Notes
You do not need to any prefab on the scene, it will be auto loaded by the script itself and assigned to do not be destroyed.

### Functions
`SimpleUIDialog.ShowToast()`

Parameters
-	Duration ( Up Time ) ( Optional )
After the up time, toast will close automatically

`SimpleUIDialog.Dispose()`

Destroys SimpleUIDialog from scene, free-ing up memory. Can be reinstantated by calling any static methods

Prefab is found at
> Assets/Resource/SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas

* To change the scaling of the UI, Modify the Scale Factor on the Canvas Scaler on the Prefab

___

![alt text](https://github.com/superlaser97/Unity-Toolbox/blob/master/Assets/SUPERLASER/AsyncSceneLoader/README_Header.PNG "AsyncSceneLoader")
## AsyncSceneLoader
Transition between scene with loading indicator

### Usage
In your script call `SceneManager.LoadScene()`

Parameters
- SceneName (string), Name of the Scene that you want to load

### Notes
Remember to Add these Scenes to the build before using it.
- AsyncLoadingScene
- Target Scenes ( Scenes you want to load )

For the Demo, *DemoScene-Start* and *DemoScene-End* is the Start point and End point of the demo respectively, while *AsyncScneLoader* is the loading scene itself.

___

![alt text](https://github.com/superlaser97/Unity-Toolbox/blob/master/Assets/SUPERLASER/DebugTools/README_Header.PNG "DebugTools")
## DebugTools
- Log to dedicated debug tools console that is accessible in runtime
- Commands that can be executed
- FPS Counter and Stats Monitor
- Save Logging to File

### Debug Tools Console Usage
To log, call `DebugTools.Log()`
DebugTools is Auto Instantiated and Initialized at the start of any scene, no prefab required on the scene.

Parameters
- Log (string) , string that you want to Log
- DebugLevel , level of debug, either NORMAL, WARNING, ERROR, COMMAND

Logging through `DebugTools.Log()` will also mirror log to `UnityEngine.Debug.Log()`

### Notes
Remember to Add these Scenes to the build before using it.
- AsyncLoadingScene
- Target Scenes ( Scenes you want to load )

For the Demo, *DemoScene-Start* and *DemoScene-End* is the Start point and End point of the demo respectively, while *AsyncScneLoader* is the loading scene itself.
