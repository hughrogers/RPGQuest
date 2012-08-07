SmoothMoves 2D Skeletal Animation Editor
for use with Unity3D

Copyright 2012 echo17


********************************************************
* For tutorials and documentation check out echo17.com *
********************************************************


Video tutorials available that show how to:

- Set up the environment
- Create atlases
- Create sprites
- Create animations
- Script animations

To read the API documentation on Mac, you can download the free program iChm at:

http://code.google.com/p/ichm/

Version History:
-------------------------------

Version 1.11.0

Bug Fixes:
- None

Features:
- Storing texture names in TextureAtlases. You will need to rebuild the atlases to reference texture names programmatically.
- Added SwapAnimationBoneTexture to change a texture programmatically for one animation on one bone.
- Added SwapBoneTexture to change a texture programmatically for all animations on one bone.
- Added SwapTexture to change a texture programmatically for all animations and all bones.
- Updated demo to not use the texture search and replace components, but instead use the new swap functions.


Version 1.10.1

Bug Fixes:
- Fixed SetAnimationBoneFrameTexture in the BoneAnimation class to properly create a trigger frame if no frame exists.

Features:
None


Version 1.10.0

Bug Fixes: 
None

Features:
- Added the ability to toggle a bone's runtime visibility in the animation editor. You can now toggle this by clicking on the eyeball until the X is shown.
- Removed HideAnimationBoneTexture and HideBoneTexture functions and replaced them with HideBone which toggles a bone's visibility at runtime.
- Added IsBoneHidden function to return whether a bone has been set hidden manually or through the animation editor.


Version 1.9.8

Bug Fixes:
- If any negative frames exist, they will be trimmed from the animation when opening the animation in the editor.

Features:
None


Version 1.9.7

Bug Fixes:
- Fixed the ReplaceBoneTexture and ReplaceAnimationBoneTexture functions to replace textures that have already had one replacement.
- If the animation scale on any bone crosses the zero mark on any property (x, y, or z), Smooth Moves will set the zero to 0.0001 to avoid GUI.matrix errors.

Features:
- Added name of bone animation data object to the animation editor window title.
- Added name of texture atlas data object to the atlas editor window title.
- Atlas editor now automatically sets the wrap mode of the atlas texture to clamp when building atlases to avoid artifacts introduced by repeating the texture.
- Added EnigmaFactory's enhanced movement code to the Knight.cs class in the demo scene.
- Added RestoreBoneTexture and RestoreAnimationBoneTexture functions to set the textures back to those set in the animation editor on bones that had replace called on them.
- Added features to the Knight Demo scene to switch weapons during gameplay.


Version 1.9.5

Bug Fixes:
- Using the rotation gizmo with only one rotation keyframe will now rotate the image in the Animation Window. You will still need more than one keyframe for the animation curve to generate for runtime, however.
- Removed "using SmoothMoves;" directive from tutorial files and playmaker scripts to avoid conflicts with other plugins.

Features:
- Animation curve needing at least two keyframe warnings will now only show on clips / bones / properties that are being mixed or exist in animations not using mixing.
- Duplicating a clip will now duplicate FPS, WrapMode, Mix, BlendMode, Layer, and BlendWeight as well as copying the mix properties of each bone.
- Added User Guide to the documentation directory (updated User Guide is also available online at echo17.com).
- Added Playmaker scripts to the Playmaker scripts directory (updated scripts are also available online at echo17.com).


Version 1.9.3

Bug Fixes:
- Fixed the initial collider size if using a mesh import scale <> 1.0f and you have a collider on frame zero of one of your bones.
- When setting atlases on multiple keyframes greater than frame zero that have not had the use keyframe type and use atlas checked, these two toggles will now be checked properly.
- Fixed some errors that were caused by overwriting frame zero by dragging other keyframes there.

Features:
- Larger input boxes on collider center and box size in keyframe properties window.
- Success dialog box popup when clicking on the update mesh in scene button from the animation editor top controls.


Version 1.9.2

Bug Fixes:
- Removed sprite creation button from atlas editor to avoid focus problems with third party plugins.
- Changed sprite texture selection to a list instead of selecting from the atlas editor to avoid focus problems with third party plugins.
- Changed texture replacement component texture selection to a list instead of selecting from the atlas editor to avoid focus problems with third party plugins.
- Prevented playing animation clips that have only one key to avoid division by zero errors.
- When dragging keyframes around, the maximum keyframe in a clip will be calculated properly for animation length.

Features:
- Recreated all assets for compatibility with newer versions of Unity


Version 1.9.1

Bug Fixes:
- Triggers will fire properly for queued animations.
- Put in a limitation on the grid size to not allow zero pixels or lower. If you want to hide the grid, just uncheck the "Show Grid" option in the settings window.
- Put in a check to be sure zoom isn't set to zero on the pivot editor before drawing to avoid lock ups when the pivot editor button is pressed.

Features:
- Ability to drag selected keyframes backward and forward in time after selecting one or more keyframes.
- Settings window now has a list of the keyframe properties that can be set for copying and pasting. You can now copy and paste any property or combination of properties.
- Insert frames (shifting future frames on all bones forward) by double clicking on a column and selecting insert frames from context menu.
- Delete frames (shifting future frames on all bones back and deleting keyframes) by double clicking on a column and selecting delete frames from context menu.
- Insert frames (shifting future frames on selected bones forward) by selecting a range of frames and clicking insert frames from context menu.
- Delete frames (shifting future frames on selected bones back and deleting keyframes) by selecting a range of frames and clicking delete frames from context menu.
- Embedded editor textures into the editor dll so there is no longer any need to mess with the resource path if you move your dll's around.


Version 1.8.0

Bug Fixes:
- Optimizations in editor to improve refresh rate when many animation clips exist.
- Image scaling will now work when mixing.
- Fixed refreshing after Undo.

Features:
- Added a bone visibility toggle to show / hide bones in the animation editor.
- Added Local Scale property to each keyframe. This differs from the image scale in that it will scale all child bones as well.
- Added local scale / image scale gizmo toggle to switch scale gizmos between the two types of scales
- Put a zero degree line in the rotation gizmo to better see the angle.
- Changed alternate contrast to be lighter background and darker gizmos.
- Zooming in the animation editor window will now center around the mouse, not the origin.
- Improved selection bounds detection when clicking on bones in animation editor window.
- Changed "Update Colors" setting in BoneAnimation to be false by default for better performance. If you want colors to be updated at runtime, you will need to manually set this to true either in the inspector at design time or in code at runtime.
- Changed max atlas size selection from a free entry integer field to a drop down list that matches Unity's texture inspector max size list.


Version 1.7.2

Bug Fixes:
- If setting the import scale before opening the animation editor for the first time on an animation, the import scale was being reset to 1.0f. The scale will now stay what it was set to before opening the editor for the first time.
- Position and scale animation curve tangents are now scaled properly to the mesh import scale.
- Warn if rebuilding an atlas will overwrite material or texture files of the same name.
- If first keyframe is a transform only, and then a later frame switches to an image, you will no longer have to explicitly set the image scale keys to ( 1.0f , 1.0f ) for the image to show in the editor.
- Moved the bone animation updates to the scene manager's lateupdate function call to avoid a one frame delay.
- Fixed a bug that was causing list enumeration errors if you try to play an animation based on a trigger in another animation.
- Fixed the position of the scroll window in the texture selector of the animation editor. It should now jump to the correct texture selector scroll location when selecting a new keyframe.
- When switching animation clips in the editor the animation preview play direction will now reset to playing forward no matter which direction is was going before the switch.
- When copying and pasting keyframes, only the keyframes that will fit in the bone list will be copied over.
- When copying last frames of an animation clip, the keyframe color is now included.
- You can now add textures with the same name from different folders to the atlas editor.

Features:
- Added default pivot editor in the atlas editor window. Using a default pivot can save you from having to constantly set your pivots when your texture changes in the animation. This also makes it easier to swap out textures at runtime since the default pivots will be used.
- Added the ability to choose to lock the pivot of a sprite or animation bone texture to the default set in the atlas or freely change the pivot.
- Added "updateColors" property to the bone animation with the default set to true. If your animation is not using color changes (mesh, bone, or keyframe color changes), you will get a performance boost by setting this value to false. This value can be toggled at design time or run time.
- Put warning icons on animation clips, bones, and bone properties if a property needs at least two keyframes to generate an animation curve.
- Hid texture selection in the animation editor window on frames where no texture is being set.
- Hid pivot selection in the animation editor window on frames where no pivot is being set.
- Added axis thickness option in the settings window.
- Better contrast for the gizmo and labels when switching to the light contrast background
- Increased max atlas size to 4096 pixels
- Strip down the editor to the bare minimum when previewing an animation for better performance.
- Various cosmetic changes including better fitting labels and tooltips on labels that are too large.


Version 1.6.1

Bug Fixes:
- Prefab Sprites will now instantiate properly. Please note the following:
	a. Changing a setting on a sprite instantiated from a prefab will change the settings on the prefab and all sprites created from this prefab.
	b. Changing a setting on a prefab sprite will change the settings on all sprites created from the prefab.
	c. To change the settings of a sprite that was instantiated from a prefab, first disconnect the sprite from the prefab using the button "Disconnect from Prefab" in the sprite's inspector.
	d. If you copy or duplicate a sprite, the original and the new sprite will share settings just like if it was instantiated from a prefab. You will need to use the "Create Seperate Mesh" button in the sprite's editor to change settings individually.

Features:
- Rearranged all menu items under a single menu of "Smooth Moves" for quicker lookup.


Version 1.6.0

Bug Fixes:
- Fixed lines overdrawing on the bone color pop-up window
- Fixed bug that sometimes causes crash in sprite pivot editor window
- The mesh won't add keyframes for image scale if the keyframe was a transform-only key

Features:
- Added keyframe color. Now you have three ways to set the color of your mesh:
	1) Mesh color: this is the default for all color in your bone animation. This color can also be set at runtime.
	2) Bone color: this color is blended with the mesh color to give each bone a unique color across all animation clips in the animation. Blending weight determines the priority of bone color over mesh color. Blending weight of zero will only show the underlying mesh color; blending weight of one will only show the bone color. This color can also be set at runtime.
	3) Keyframe color: set per keyframe within an animation clip, this color is blended with the result of the mesh / bone color blending. Blending weight determines the priority of keyframe color over the blended mesh / bone color. Keyframe color is proritized by the animation clip layer if mixing multiple animations. Animation clips in lower layers will ignore keyframe color if a higher layer also has a keyframe color curve. The keyframe color cannot be set at runtime like the mesh and bone colors.
- Added normals to mesh so that lighting shaders will now work.
- When recreating the mesh, only the root child object (and its children) are destroyed and remade. This will allow you to attach transforms to a bone animation without the gameobjects being destroyed.
- Added mesh import scale in the inspector of the bone animation data asset. This works similar to an FXB import scale in that it will scale all bones and vertices once at instantiation of the animation.
- Added "Duplicate Previous Keyframes in Column" in the timeline header to copy the previous keyframe properties for keys at a certain frame.
- Added "Duplicate First Keyframes in Column" in the timeline header to copy the first keyframe properties for keys at a certain frame.
- Smarter positioning of the animation window when opening the texture selection or texture pivot windows in the animation editor.
- When selecting a bone, the current frame for that bone is also selected instead of jumping back to the first frame.
- Double-clicking the timeline will select an entire column of frames.
- Double-clicking a bone will show the textbox for renaming.
- Double-clicking an animation clip will show the textbox for renaming.
- Added the bone name to the bone color pop-up window.
- The timeline shortcut keys B, P, and T are all global, so you can press them while any section of the editor has focus to create keyframes
- Added FunWithColors scene to show off the color functionality
- Added FunWithLights scene to show off the light functionality


Version 1.5.0

Bug Fixes:
- Copy and paste now work on bone names.
- Copy and paste now work on animation clip names.
- The animation curve editor window will now stay on top of the editor window when a new animation curve is selected.
- The texture selection window will now automatically jump to the the selected texture when switching keyframes.
- Duplicate bone names are no longer allowed.
- Duplicate animation clip names are no longer allowed.

Features:
- Added bone color / blending in the editor. Bones will now show color, blended with the mesh's base color depending on the blend weight of the bone color.
- Added the ability to set a bone's color programmatically.
- Added the ability to flash a bone from one color to another programmatically. Duration and repetitions allow flexibility in what the flash looks like.
- Arrow Keys will move the selected timeline frame around, even from other windows so you can quickly change frames while modifying a keyframe's properties.
- "B" shortcut key when in the timeline window will create new blank keyframe (or multiple keyframes depending on the selection).
- "P" shortcut key when in the timeline window will create new pos - rot keyframe (or multiple keyframes depending on the selection).
- "T" shortcut key when in the timeline window will create new texture keyframe (or multiple keyframes depending on the selection).


Version 1.4.0

Bug Fixes:
- Rewrote the animation event system to a custom watcher that will allow Smooth Moves to run on mobile devices
- Added a material change when setting a single frame texture replacement in case the texture is in a different material than the pre-existing image.
- Fixed the atlas creator to import new images with a source maximum size of 2048 instead of 1024, so that the final atlas can be larger than 1024 on non-iOS devices.

Features:
- Added a hidden keyframe in the root so that you don't have to explicitly set a second keyframe for animations without animation curve properties, like a texture switching animation without movement, rotation, or scale. This also allows the root to offset and rotate an entire animation.


Version 1.3.0:

Bug Fixes:
-Copying keyframes now only copies the position, rotation, and scale so that other keyframe data is not lost when copying over frame zero of a bone.
-When using the mouse wheel to scroll in the timeline, the gizmos will only show if the timeline cursor is over the currently selected frame.
-When previewing an animation, the ability to select bones in the animation window is disabled.
-Prefabs will now regenerate animation states when instantiated
-Labels can now be seen in Unity Pro due to new color theme.

Features:
-"R" keyboard shortcut for renaming bones and animation clips.
-When renaming bones and animation clips the item name is selected for easy replacement.
-Bones and animation clip lists now scroll with the mouse wheel
-Textures are shown in alphabetic order in the texture atlas editor and the texture selector window inside of the animation editor.
-Texture names are displayed beneath textures in the atlas editor and the texture selector window inside of the animation editor.
-Dark theme to match Unity Pro
-Contrast button that will toggle the darkness of the background of the animation and pivot windows.
-Z position with animation curves. Depth still determines the draw order, but z position can be used to interact in three dimensions with other 2D objects. z position also allows perspective shifts to give your animations more depth if using a perspective camera. The pos-rot set keyframe shortcut still only sets the x & y positions along with the rotation without setting a z position.
-Bone animation data stats in the inspector.

Version 1.1:

- Initial Release