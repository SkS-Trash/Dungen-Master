AudioVideoOptionsMenu

Version :
 unity 2022 (and up)
   4.1   Removed exedentely included dependencies, fixed deprecated code.

 unity 2019:
   4.0   Completely rewritten Scripts and a new Look for the UI menus, using TMP.
   3.2   Added Universal Render Pipeling compatible menu.

 unity 2017 : 
   3.1   Added a revert to default graphics option.
   3.0   Added Audio options menu and a menu system
   2.0   New ini file save system.

 Unity 5.6 : 
   1.2   removed duplicate resolution option buttons in the drop down menu. 

 Unity 5.5 : (The Unity 5.5 version does not work on Unity 5.4)
   1.1   small fixes.
   1.0   added Shadow Resolution and Texture Quality sliders. (added a sample menu of my "Royal UI" package available on the asset store)
   0.5.3 small adjustments/finetuning 
         removed asigning camera to canvas set to "ScreenSpace-Camera".

 Unity 5.4 :
   1.1   small fixes.
   1.0   added Shadow Resolution and Texture Quality sliders. (added a sample menu of my "Royal UI" package available on the asset store)
   0.5.3 small adjustments/finetuning 
         removed asigning camera to canvas set to "ScreenSpace-Camera".

 Unity 5 through 5.3 :
   0.5.2
   0.5.1 added a "Toon" version of the menu.
   0.5   small fixes/optimizations.
   0.4   added option to save to ini file.
   0.3   made resolution menu scrollable.
   0.2   fixed asigning camera to canvas when loading new scene.
   0.1   initial release.
      

!! First thing to do is !!
Go to the "_Packages" folder and install the >BuitIn< or >URP< pack Depending on wich one you want to use.

All you need to do is drag the "_OptionsMenu_Combined" prefab into the first scene of your Game/Project.
!!Please make sure you have a "EventSystem" added to every scene or the canvas will not work.

The URP version and Audio menu require a little more setup, Instructions are included

(Adding the prefab to a later scene is fine too, but be aware the menu is set to "DontDestroyOnLoad()".
Reloading a scene/level that is build with the menu already in it will add an aditional menu prefab to the scene)

Select how you want to save (playerPrefs or .ini file). 
Saving to .ini file will create a text file in your build projects folder called "QualitySettings.ini".
(When running in the unity editor it will save to the main asset folder of your project).

To open the Combined menu in game press Escape (but you can change this to any Key/Button you want).

On each script, in the editor there is an Used/notUsed setting for every menu option.
Options set to UnUsed can savely be disabled/Removed from the menu UI;

-If you like the asset, rating it on the Asset store would be greatly appreciated.-
-If you don`t like the asset letting me know why would be equally helpful.-

for any questions, comments, Bugs or suggestions feel free to contact me.