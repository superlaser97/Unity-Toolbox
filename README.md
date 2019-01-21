# Unity-Toolbox
Documentation on what and how to use these tools

## Included Tools ( Big ones )
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

## SimpleUIDialog
Easy way to show beautifully designed dialog with deletaged buttons. Supports multiple dialog calls at the same time

### Notes
You do not need to any prefab on the scene, it will be auto loaded by the script itself and assigned to do not be destroyed.

### Functions
`SimpleUIDialog.ShowDialog()`

Parameters
-	Title
-	Content
-	Buttons (Delegates and Button Text)
-	Show “X” Button
-	Highlighted Button
After any button is clicked, the dialog will automatically close

`SimpleUIDialog.Dispose()`

Destroys SimpleUIDialog from scene, free-ing up memory. Can be reinstantated by calling any static methods

Additional Settings is on the Prefab at
> Assets/Resource/SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas
-	Accent Color (Highlight color)
-	Animation Spd

* To change the scaling of the UI, Modify the Scale Factor on the Canvas Scaler on the Prefab

___


## SimpleUIToast
Easy way to show Toast with nice animations. Supports multiple toast at the same time

### Notes
You do not need to any prefab on the scene, it will be auto loaded by the script itself and assigned to do not be destroyed.

### Functions
`SimpleUIDialog.ShowToast()`

Parameters
-	Duration ( Up Time )
After the up time, toast will close automatically

`SimpleUIDialog.Dispose()`

Destroys SimpleUIDialog from scene, free-ing up memory. Can be reinstantated by calling any static methods

Prefab is found at
> Assets/Resource/SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas

* To change the scaling of the UI, Modify the Scale Factor on the Canvas Scaler on the Prefab

___

## AsyncSceneLoader
Transition between scene with loading indicator

### Usage
In your script call `SceneManager.LoadScene("SceneName")`, where the parameter is the name of the scene

### Notes
Remember to Add these Scenes to the build before using it.
- AsyncLoadingScene
- Target Scenes ( Scenes you want to load )

For the Demo, *DemoScene-Start* and *DemoScene-End* is the Start point and End point of the demo respectively, while *AsyncScneLoader* is the loading scene itself.
