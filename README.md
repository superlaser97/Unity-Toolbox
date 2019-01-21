# Unity-Toolbox
Documentation on what and how to use these tools

## Included Tools ( Big ones )
- SimpleUIDialog
- SimpleUIToast
- DebugTools
  - FPS Counter
  - Console
- Versioning
- Feature Flags
- AsyncSceneLoader

___

## Requirements: 
-	Unity 2018.1 and above
-	Half a brain

___

## SimpleUIDialog
Easy way to show beautifully designed dialog with deletaged buttons. Supports multiple dialog calls at the same time

### Usage
In your script call `SimpleUIDialog.ShowDialog()`
You do not need to any prefab on the scene, it will be auto loaded by the script itself and assigned to do not be destroyed.

### Functions
`ShowDialog()`

Parameters
-	Title
-	Content
-	Buttons (Delegates and Button Text)
-	Show “X” Button
-	Highlighted Button
After any button is clicked, the dialog will automatically close

`Dispose()`

Destroys SimpleUIDialog from scene, free-ing up memory. Can be reinstantated by calling any static methods

Additional Settings is on the Prefab at
> Assets/Resource/SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas
-	Accent Color (Highlight color)
-	Animation Spd

* To change the scaling of the UI, Modify the Scale Factor on the Canvas Scaler on the Prefab

___


## SimpleUIToast
Easy way to show Toast with nice animations. Supports multiple toast at the same time

### Usage
In your script call `SimpleUIToast.ShowToast()`
You do not need to any prefab on the scene, it will be auto loaded by the script itself and assigned to do not be destroyed.

### Functions
`ShowToast()`

Parameters
-	Duration ( Up Time )
After the up time, toast will close automatically

`Dispose()`

Destroys SimpleUIDialog from scene, free-ing up memory. Can be reinstantated by calling any static methods

Prefab is found at
> Assets/Resource/SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas

* To change the scaling of the UI, Modify the Scale Factor on the Canvas Scaler on the Prefab
