
Thank you for purchasing ORK Okashi RPG Kit!
Visit http://www.rpg-kit.com for the latest news and tutorials.


-------------------------------------------------------------------------------------------------------------------------------------------------------
Documentation
-------------------------------------------------------------------------------------------------------------------------------------------------------

You can find the documentation in the ORK Wiki:

http://rpg-kit.com/wiki


-----------------------------------------------------------------------------------
Demo project
-----------------------------------------------------------------------------------

Download the demo project here:

Windows:
http://rpg-kit.com/download/ork_demo_project_win.zip

Mac:
http://rpg-kit.com/download/ork_demo_project_mac.zip

The demo project doesn't include sources.
Simply import the latest ork_cs.unitypackage to get the demo running.


-----------------------------------------------------------------------------------
How To
-----------------------------------------------------------------------------------

The packages must be imported directly into your projects Assets folder.

The following folders must be directly in your Assets folder of your Unity project.
Don't put them into a sub-directory.

Resources
Gizmos
RPG Kit

If you're updating an existing project:
Don't import the Resources folder, this will override your ORK project data!


-----------------------------------------------------------------------------------
Folder descriptions
-----------------------------------------------------------------------------------

Resources
Contains ORKs Resources folder structure and base project data.

Gizmos24
Contains ORKs gizmo icons in 24*24 pixels.
Rename to Gizmos if you want to use 24x24 pixel icons.

Gizmos
Contains ORKs gizmo icons in 32*32 pixels.

RPG Kit
Contains ORKs script files in C#.

-----------------------------------------------------------------------------------
Version changes
-----------------------------------------------------------------------------------

Version 1.2.0
- new: Unity 3.5 compatibility.
- new: Difficulties (Project Editor): Create different difficulties for your game.
- new: Game Settings: Game statistic settings.
- new: Game Settings, Load Save HUD: Data encryption option.
- new: Game Settings: DEFAULT type player control jump settings now have a ground angle limit for allowing jumps.
- new: Game Settings: Advanced game over settings - display a retry/load/exit choice dialogue.
- new: Game Settings: Option to spawn party only in battle areas (REALTIME only).
- new: Load Save HUD: Save game options - choose which data will be saved (e.g. variables, inventory, etc.).
- new: Load Save HUD: Auto save settings.
- new: Save Points: New save point type setting: SAVE_POINT, AUTO_SAVE and RETRY_POINT.
- new: Save Points: Variable conditions and set variables when saving (only for AUTO_SAVE and RETRY_POINT).
- new: Items: Useable items can call global events (only if useable in field and not in battle).
- new: Characters, Enemies, Status Effects, Skills, Weapons, Armors, Classes: Extended bonus and difficulty settings.
- new: Characters, Enemies: AI behaviour difficulty settings.
- new: Characters, Enemies: Status value time change settings - CONSUMABLE type status values can be changed over time in battle and field (character only).
- new: Characters, Enemies: Set a child of the prefab as root object (e.g. when the prefab root isn't the moving object).
- new: Characters, Enemies, Weapons: Battle/field animation speed settings. Use formulas to change playback speed of animations.
- new: Characters, Enemies: Custom animation settings - add animations and let ORK handle layer and speed.
- new: Enemies: Class level setting.
- new: Main Menu: Difficulty selection option for new game.
- new: Classes: Class levels and class status development settings.
- new: Weapons, Armors: Minimum class level setting. Equipment skills class level requirement option.
- new: Status Effects: Effect prefabs have a rotation offset setting.
- new: Status Effects: Auto apply/remove - level and class level requirement options.
- new: Status Effects: Option to stop status values from changing while the effect is applied (e.g. immortality).
- new: Status Effects: Block base attacks and skill types settings. Blocked attacks and skills (by type) wont do damage or change status effects.
- new: Status Values: Count settings for battle text settings - the damage/heal text display can optionally count to the actual value.
- new: Status Values: Class level up option in EXPERIENCE type.
- new: Formulas: Use class level option.
- new: Formulas: Min/max piece and formula result settings.
- new: Formulas: Check value can end calculation.
- new: Formulas: Formula step grouping - calculate partial results of grouped steps, e.g. 5*(10-5)
- new: HUDs: New HUD element type USED_TIMEBAR, representing the amount of timebar already used by a characters actions.
- new: Battle Texts: Local space option for offsets.
- new: Battle Text: Class level up text settings.
- new: Battle End: Class level up text settings.
- new: Game Events: Save as option.
- new: Game Events: Show choice step - add item option. Add an item, weapon or armor to the party inventory.
- new: Game Events: Change class step - additional settings for class level, skills and status bonuses.
- new: Game Events: Level up step - class level up setting.
- new: Game Events: Check level step - check class level setting.
- new: Game Events: Parent object step - advanced position and rotation settings.
- new: Game Events: Rotation step - interpolation and set axis options.
- new: Game Events: Move to prefab step - move an actor or prefab to the position of a prefab.
- new: Game Events, Battle Animations: Wait for button step - wait if a defined button is pressed, if pressed, next step is executed, if not, next step fail.
- new: Game Events, Battle Animations: Check difficulty step.
- new: Game Events, Battle Animations: Custom statistic step - change a custom game statistic.
- new: Game Events, Battle Animations: Clear statistic step - clear (reset) the game statistics.
- new: Battle Animations: Call animation step - call another battle animation to be performed (using the current animations user/target).
- new: Battle Animations: Multiple calculation steps are now allowed per animation (i.e. multi-damage possible), new damage factor option.
- new: Battle Animations: Check user step - check if the user is a character or enemy.
- new: Battle Animations: Damage multiplier step - set the multiplication factor for action calculations.
- new: Battle Animations: Rotation step - interpolation option.
- new: Battle Animations: Mount camera step - mount the camera to a combatant, prefab or the battle arena, or unmount it.
- new: Battle Animations: Move to step - move to prefab option.
- new: Battle System: Multi choice settings (system type ACTIVE only). A combatant can select multiple actions before using them.
- new: Battle System: Menu border setting for system type ACTIVE - the battle menu will be called when the timebar reaches the set value.
- new: Battle System: Max timebar setting for system type ACTIVE - timebar can increase after reaching the action border.
- new: Battle System: Optionally add a counter to enemy names when multiple enemies of the same type participate in a battle - either NONE, LETTERS (A, B, C, ...) or NUMBERS(1, 2, 3, ...). Only for system type TURN and ACTIVE.
- new: Battle System: Party/enemies advantage settings - the party or enemies can get advantages at the start of the battle (chance). Timebar, CONSUMEABLE status values and status effect settings.
- new: Battle System: Default battle spot settings - set default battle spots for TURN and ACTIVE battles (standard battle, party/enemies advantage). If no battle spot in a Battle Arena is selected, the default spots are used.
- new: Item Collectors: Local space and rotation offset settings.
- new: Battle Agents: Using maximum distance to battle arena is now optional (default true).
- new: Battle Areas: Spawn enemies on entering the area (else they'll spawn when the scene is loaded). Defeated enemies will be respawned when reentering the area.
- new: Battle Arenas: Additional battle gain settings - add money, items, weapons or armors to the enemy drops of a battle.
- new: Battle Arenas: Battle advantage settings - enable/disable battle advantages and own chance settings for each battle.
- new: Battle Arenas: Party/enemies advantage battle spot settings.
- new: Random Battles: Create areas (with triggers) in which random battle encounters occur.
- new: Mouse/Touch Controls: Select when to react on click/touch, either START, MOVE or END. Not available for mobile camera (forced MOVE).
- change: Level up: Leveling up can now level up multiple levels, if enough experience was earned.
- fix: Layer mask settings now use the correct layer, you may need to recheck your layer settings (MOBILE player controls, target raycasts, battle area spawns, item collector ground placement).
- fix: Battle End: Gaining weapons and armors from battles displayed the wrong texts.
- fix: Game Events: Set to position step - using position (not waypoint) without a waypoint in the list caused an error.
- fix: Battle System: Combatant killed by a status effect could prevent the battle from going on.
- fix: Items: Copying an item in the editor didn't copy the prefab.

Version 1.1.5
- new: Enemies: New aggressive settings ALWAYS, DAMAGE, SELECTION and ACTION. Select when the enemy will become aggressive.
- new: Game Settings: GUI scale mode setting, either STRETCH_TO_FILL, SCALE_AND_CROP, SCALE_TO_FIT or NO_SCALE.
- new: Game Settings: Add interaction controller settings. Automatically add a prefab used as interaction controller to the player.
- new: Battle System: Use child object of party target for cursor placement (REALTIME).
- new: Battle Menu: Target selection cursor can use child object of the target.
- new: Battle Menu: Target blink settings - use current setting. Blinking will use the current color value as start and return to it.
- new: Battle Text: Target flash settings - use current setting. Flashing the target will use the current color value as start and return to it.
- new: Game Events: Show choice step - optional variable conditions for each choice.
- new: Game Events, Battle Animations: Fade object step - set property settings. Set which material property to change, and if it's a float.
- new: Game Events, Battle Animations: Fade object step - shared material option. Fading/flashing can use a shared material, this will change the material in the project.
- new: Game Events, Battle Animations: Fade object step - use current setting. Starts fading/flashing from the current color value, flashing returnts to the value.
- new: Battle Agents: Wait time and change time settings. Set the time the enemy will take to switch between following and returning, and a wait time between those modes.
- new: Battle Arenas, Event Interactions, Item Collectors, Music Players, Save Points, Variable Checkers: Start type KEY_PRESS has an in trigger option - will only start when player is within the trigger of the game object.
- change: Item Collectors: Start type KEY_PRESS is now supported.
- change: Battle Arenas: Start type KEY_PRESS is now supported. Start type INTERACT now supports mouse/touch and has max. distance settings.
- change: Music Players: Start types KEY_PRESS and DROP are now supported.
- change: Save Points: Start type KEY_PRESS is now supported.
- change: Variable Checkers: Start types KEY_PRESS and DROP are now supported.
- fix: Battle Agents: Enemy still played run animation when he returned to the starting point.
- fix: Battle System: Dynamic combat could stop characters from performing actions.

Version 1.1.4
- new: Base Attacks, Skills: Steal (item/equipment, money) settings.
- new: Base Attacks, Skills: Blockable option in status value consume setings.
- new: Base Attacks, Skills: Ignore defend, element, race and size option in status value consume settings.
- new: Items: Stealable setting, item can be stolen by enemies if enabled.
- new: Races (Project Editor): Create different races for characters and enemies. Races work similar to elements and change attack damages.
- new: Sizes (Project Editor): Create different sizes for characters and enemies. Sizes work like races (similar to elements) and change attack damages.
- new: Weapons: Weapons now have element effectiveness settings.
- new: Status Effects, Skills, Weapons, Armors: Race/size damage factor change settings.
- new: Classes, Enemies: Race/size damage factor settings.
- new: Battle AI: Check race/size condition.
- new: Characters, Enemies: Block chance. Chance for blocking attacks completely.
- new: Status Effects, Weapons, Armors, Characters, Enemies: Block, steal item, steal money chance bonus settings.
- new: Status Effects: Auto apply/remove race/size requirement.
- new: Game Settings: Default player control now has 'use camera direction' settings. Deactivate for player movement ignoring camera view.
- new: Battle Text: Block text settings.
- new: Battle Text: Steal item/money and steal fail item/money text settings.
- change: Project Editor: Tab order reorganized. Character/enemy race/size filter.

Version 1.1.3
- new: Characters, Enemies: Field animations minimum fall time settings. Set the minimum time the character/enemy has to fall to play a land animation.
- new: Load Save HUD: Save game to PlayerPrefs or file option.
- new: Teleports (Project Editor): Create teleport targets for fast travel across your game world.
- new: Base Attacks (Project Editor): Create base attacks to use for characters, enemies and weapons. Your old attacks will get converted into the new tab.
- new: Base Attacks: Consume item settings. Attacks can remove (and add) items from the inventory. The attack can't be performed if the items aren't in the inventory.
- new: Battle Cam: Limit rotation settings.
- new: Battle Cam: Simple look at child settings.
- new: Battle Text: Use child of combatant as base position.
- new: Battle Text: Extended text settings.
- new: Battle Text: Spawn prefab and play audio settings.
- new: Game Events: Teleport step. Spawn party to a teleport target.
- new: Game Events: Teleport choice step. Show a choice dialogue with all available teleport targets and spawn party at the selected target.
- new: Game Events: Change character name step. Set the name of a character using either a game variable or a defined text.
- change: Characters, Enemies, Weapons: Base attack settings are now done in the 'Base Attack' tab. Old settings will be imported into new attack data.
- change: Battle Text: Miss, cast cancel and level up texts are now done in their battle text settings directly.
- change: Battle Text: Status value damage/refresh texts are now done in the Status Value tab of the Project Editor.
- fix: Game Events: Animations will now use the first animation component found on a game object (and its children).
- fix: Field Animations: Idle animation stopped when falling from short heights (i.e. playing land animation without fall animation).

Version 1.1.2a
- fix: Status Values: Adding and saving a new status value could have thrown an error message under certain circumstances.

Version 1.1.2
- new: Global Events (Project Editor): Use game events as global events, they can run in all scenes without adding them to a game object (e.g. hour system).
- new: Characters, Enemies: Critical hit chance.
- new: Base Attacks (Characters, Enemies, Weapons): Critical hit damage settings.
- new: Base Attacks (Characters, Enemies, Weapons): Attack/critical audio settings. Select an audio clip that will be played when damage is dealt.
- new: Status Effects, Skills, Weapons, Armors, Characters, Enemies: Extended bonus settings (hit, counter, critical, escape, speed).
- new: Items, Skills: Use audio clip settings. Select an audio clip that will be played when the item/skill is used.
- new: Game Settings: Pause key settings.
- new: Game Settings: Mobile camera now has zoom key settings (e.g. for mouse wheel zoom).
- new: Game Settings: First person camera settings. Default player control now has first person option.
- new: Dialogue Positions: Select first option, a choice must be selected first to be accepted by mouse click.
- new: Equipment Viewer: Link children of equipment and character to match position/rotation (e.g. for sleeves of an armor).
- new: Game Events: Call global event step.
- new: Game Events, Battle Animations: Set/get/has PlayerPrefs steps. Use Unitys PlayerPrefs to communicate with other products.
- fix: Game Events: Learning a passive skill didn't give status value bonuses immediately (only after changing equipment or level up).

Version 1.1.1
- new: Skills: Change targets battle order position in turn based combat (no dynamic combat).
- new: Characters, Enemies: Land animation (field and battle).
- new: Enemies: Change game/number variables on death.
- new: Game Settings: Freeze pause setting, game freezes at pause.
- new: Game Settings: Mobile camera supports mouse control, inverting, more settings.
- new: No Click Move: Set up colliders/triggers to block mobile control on areas.
- new: Battle System: Freeze action setting for real time battles, game freezes on battle system call.
- change: Animations will now use the first animation component found on a game object (and its children).
- fix: Battle Agents: Battle agents didn't have move animations in battles.

Version 1.1.0
- new: Battle System: New type REALTIME. Enables real time battles.
- new: Battle System: Real time battle controls and party target settings.
- new: Battle Text: Select the info texts that should be displayed - skills, items, defend, escape and counter.
- new: Battle End: Split experience option.
- new: Battle End: Get immediately option - receive victory gains as soon as an enemy dies.
- new: Battle End: Drop items/money options.
- new: Battle Area: Used for real time battles.
- new: Enemy Spawner: Used for real time battles.
- new: Damage Dealers/Zones: Used to do damage when not using calculation steps in battle animations.
- new: Skills, Items: New target options SELF and NONE.
- new: Skills, Items: Use range settings.
- new: Characters, Weapons: Animation layers.
- new: Characters, Enemies: AI mover settings.
- new: Characters, Enemies, Weapons: Add multiple base attacks to create combo attacks.
- new: Characters, Enemies, Weapons: Base attack range settings.
- new: Characters: AI settings.
- new: Characters: Control map settings.
- new: Battle AI: Check for a combatants death.
- new: Enemies: Item drop quantity settings.
- new: Enemies: AI can have multiple actions.
- new: Game Settings: Player control settings.
- new: Game Settings: Camera control settings.
- new: Game Settings: Switch player and party spawn/follow settings.
- new: Game Settings: Item collection pickup animation setting.
- new: Game Settings: Money collection settings.
- new: HUDs: New click option BATTLEMENU. Calls battle menu on HUD click in real time battles.
- new: Battle Animations: Calculation needed option, sets if a battle animation forces attack calculation.
- new: Battle Animations: Activate damage step.
- new: Battle Animations: Restore control step.
- new: Battle Animations: Look at step.
- new: Battle Animations: Add/remove/check item step.
- new: Battle Animations: Spawn prefab, send message, add/remove component, rotate camera around, play/stop sound, fade object, set to position, move to, rotate to, look at steps have path to child object settings
- new: Equipment Viewer: Character settings removed. The character is automatically recognized.
- new: Item Collector: Money settings.
- change: JavaScript support removed. ORK is only available in C# from now on.
- change: Battle AI: Enemy AI is now called Battle AI, as it can also be used by characters.
- change: Battle Arena: Use battle animator option removed.
- fix: Battle Animations: Destroy prefabs option hasn't been saved.

Version 1.0.9
- new: Battle System: Dynamic combat option for turn based battles with active command and active time battles.
- new: Battle System: Battle cam settings, override battle animation camera steps, rotate around the arena, current active combatants, target selection and menu user.
- new: Battle Text Settings: Mount texts (damage, effects, etc.) to combatants.
- new: Battle Text Settings: Cast cancel and level up text settings.
- new: Battle Animations: Spawn prefab mount option, mount prefabs to their target.
- new: Battle Animations: Set/remove/check variable and number variable steps.
- new: Battle Animations: Add/remove component steps.
- new: Battle Animations: Set to position step.
- new: Battle Animations: Move to step enables moving by speed (and using battle move speed from character/enemy).
- new: Characters: Auto attack settings (for dynamic combat).
- new: Characters, Enemies: Battle move speed settings.
- new: Click Player Controller: Ignore Y distance option.
- new: Status Effects, Weapons, Armors: Reduce move speed settings.
- new: Formulas: User number variables (VALUE) and user/target level (STATUS) in formulas.
- new: Formulas: Sum option for rounded calculation steps, sums up the current formula value (e.g. for 12: 1+2+...+12=78).
- new: Formulas: POWER_OF calculation operation, the current formula result will be raised to power of the next steps value.
- new: Formulas: LOG calculation operation, the current formula result will be used in a logarithmic calculation with the next steps value as base.
- new: Formulas: Random with formulas as min/max values can now use the minimum or maximum of both formulas instead of taking a random value between them.
- new: Game Settings: Character plus/minus keys, used to change the active battle menu if more than one characters turn is ready.
- change: Battle Animations: Send/broadcast message, spawn prefab, set/fade/rotate camera, play sound and fade object steps can have user, target, arena and prefab as used objects.
- change: Skill casting time is available in turn based battles with dynamic combat.

Version 1.0.8
- new: Official iOS support.
- new: Status Effects: Reflect skills option, skills set to reflectable will be reflected back on the caster.
- new: Formulas: Random formulas, select formulas for the min and max random values.
- new: Formulas: Rounding, each calculation step can round the current formula value (floor, ceil, round).
- new: Skills: Skill combos, set skills that must be used before a skill can be used.
- new: Skills: Passive skills, add bonuses to status values, elements, hit/escape/counter chance and auto effects, effect immunity.
- new: Skills: Cast time, cancelable casting.
- new: Skills: Reuse time.
- new: Skills: Skill levels.
- new: Skills: Reflectable option, skills will be reflected by reflect status effect.
- new: Items, Equipment, Classes, Characters, Enemies: Skill levels.
- new: Skills, Items, Weapons, Characters, Enemies: Attack efficiency, skill cast canceling.
- new: Game Settings: Select key handling, either button down/up or key down/up.
- new: Game Settings: Skill level change keys, skill level name settings.
- new: Game Settings: Item collection choice settings.
- new: HUDs: Display skill cast time.
- new: Game Events: Check/set player step. Check/set status value step.

Version 1.0.7
- new: Status Effects: Other status effects can be added/removed on effect end.
- new: Game Events: Event steps are copyable.
- new: Game Events: Check and change class steps.
- new: Battle Animations: Animation steps and complete battle animations are copyable.
- change: Unity 3.4 compatibility
- fix: Battle Menu: Characters with skills only through equipment have skill menu now.

Version 1.0.6
- new: Official Android support.
- new: Simple touch player and camera controls (Click Player Controller, Touch Camera).
- new: ORK GUI System: Alternative (optional) GUI system to replace Unity's GUI system.
- new: Game Settings: GUI settings.
- new: Dialogue Positions: Recalculate button in editor, resizes position, size, padding and spacing of all dialogue positions for a new screen size.
- new: HUDs: Recalculate button in editor, resizes position, size and spacing of all HUDs for a new screen size.
- new: HUDs: Display game/number variables.
- new: HUDs: Clickable HUDs for starting interactions or calling the ingame menu or a specific menu screen.
- new: Game Events: Camera events can use actor position/rotation to set/fade a camera.
- new: Status Effects: Auto apply/remove of effects when certain status value, skill or element conditions are met.
- new: Status Effects: Spawning prefabs when effect is applied.
- new: Characters, Enemies: 20 indexed audio clips for easier battle animation sharing.
- new: Battle Animations: Play indexed audio clips.
- new: Weapons, Armors: Block equipment parts, e.g. a 2handed-sword can block the shield slot when equipped.
- new: Battle Menu: Optional back button for skill, item and target menu.
- new: Main Menu: Optional auto call for the main menu when the menu scene is loaded, no MainMenuCall component needed.
- change: Components Menu: Reorganized components in different groups (events, scenes, battles, etc.).
- fix: Game Events: Speaker portraits don't distort on different screen ratios any longer.
- fix: Weapons: Overriding idle and run animations is working now.
- fix: Battle Animation Editor: Saving and reloading animations with wrong (folder) or empty prefabs could destroy it's animation steps.

Version 1.0.5
- new: Dialogue Positions: Drag window option
- new: Items, Weapons, Armors: Dropable option
- new: Battle Animations: Send/broadcast message steps
- new: Battle System: Drag and double click options for battle menu
- new: Game Settings: Pause time option, drop settings, money/time text settings
- new: Main Menu: Exit to main menu settings
- fix: Building: Building a game throwed an error because of editor code outside editor folder

Version 1.0.4
- new: Game Events: Dialogue/Choice can display a portrait (image) for the speaker.
- new: Game Events: Parent object step, parent/unparent actor's or prefabs.
- new: Event Editor: Improved event step workflow, add or move steps at certain positions.
- new: Dialogue Positions: Name box GUISkin.
- new: Formulas: If-condition, formula step can be made dependent on a certain value.
- new: Weapons: Now can overwrite all of a characters animations.
- new: Game Settings: Set the minimum and maximum for chance checks (e.g. hit chance, escape chance, etc.).
- new: Gizmo: Music Player now has a gizmo icon.
- fix: Event saving on Mac OSX: When first saving/loading an event, the correct subfolder in your project is selected.
- fix: Save game loading: Loading a save game with multiple known item recipes caused an error.
- fix: Characters/Enemies: Base animation names now can include spaces.

Version 1.0.3
- new: Item Recipes: Combine items, weapons and armors to create new items, weapons or armors.
- new: Game Events: Item recipe steps.
- new: Number Variables: use the new number variables in the event system and variable conditions to e.g. create a calendar or hour cycle system.
- new: Game Events: Number variable steps.
- new: Game Events: Autostart events have now a repeat option.
- new: Game Events: dialogues have now a close after time option (including optional blocking of the accept key).
- change: Dialogue display: positions of different dialogue elements are now calculated once, not every GUI call (performance optimization).
- fix: deleting a language caused errors in some editors.

Version 1.0.2
- new: Skills, Items, Weapons, Armors: filter the Project Editor list by type (skill type, item type, equipment part).
- new: Gizmo Icons: gizmo icons for better scene building, in 24x24 or 32x32.
- new: Characters: new 'no revive' option, character can't be revived if set.
- new: Characters: new 'leave on death' option, character is removed from party when dead.
- new: Items: items can set or remove game variables.
- new: Dialogue Positions: choice column options, create horizontal, vertical or grid choice dialogues.
- new: Spawn Points: place on ground can use an offset.
- new: Place On Ground: can use an offset.
- new: Game Events: Destroy Player Step, remove the player or whole party from the scene.
- change: Game Events: Events that don't block the controls can start in every game mode.

Version 1.0.1
- new: C# version added
- new: Project Editor > Items: Prefab selection added. Item prefabs are stored in Resources/Prefabs/Items
- new: Project Editor > Game Settings: Item collection settings added. Set dialogue position and text for item collector component
- new: ItemCollector component: Easy item pickup, select item/weapon/armor and number, component will display prefab and handle pickup event.
- new: ItemCollectorPrefab: Prefab with correctly setup item collector component for confortable use in scenes.
- new: Game events: New start type KEY_PRESS. Event will be started when a certain key is pressed, regardless of the position of the event. (Useful for e.g. creating a camera system with game events.)
- change: Project Editor > Equipment Parts: A newly added equipment part isn't automatically enabled in Weapons/Armors/Classes.