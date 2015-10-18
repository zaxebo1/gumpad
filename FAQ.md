#summary Frequently asked questions.
#labels Featured,Phase-Support




---


## I have text encoded in ITRANS that is not converting correctly to the Indian language I chose. What am I missing? ##

There are a few differences between the built-in GumPad mapping scheme and ITRANS. Load the ITRANS map located in maps\itrans.map in your GumPad installation folder to convert text encoded using the ITRANS scheme. You can load itrans.map from the Conversion Map screen which is located under the Preference->Map menu. You can revert to the GumPad built-in scheme at any time by using the Reset button on the Conversion Map screen.

## I tried pasting and converting text encoded in English with diacritics to Devanagari. There are many characters that are not displayed correctly. How do I fix this? ##

Firstly, when converting from Extended English to Devanagari or another Indian language, make sure that the "Convert from Extended Latin" checkbox is checked in the Convert menu.

Secondly, check if the text that was pasted is Unicode encoded. There are several websites that serve Indic text in Extended Latin that is encoded in non Unicode encodings. If this is the case, convert the non Unicode encoded text to Unicode and the convert it to Devanagari or another Indian language in GumPad.

If certain letters are not getting converted correctly, check if the extended Latin character mapping scheme in GumPad is different from the source encoding for these characters. If that is the case, you can create a custom map and load it into GumPad to complete the conversion. Look at maps\gumpad.map and maps\itrans.map in your GumPad installation folder for examples of map files.

## Where is the installation folder? ##

The GumPad installation folder is the directory in which GumPad was installed. If you chose the default location at install time, it should be located under C:\Program Files\GumPad.

## I customized the conversion map from the Preferences->Map menu. How can I undo my customizations and revert to the built-in mappings? ##

Go to the Conversion Map screen from the Preferences->Map menu and click on the "Reset" button. This will reset the map to the built-in scheme.

## Will a custom map be picked up automatically if I simply replace a newer version of it in the maps folder? ##

gumpad will not automatically pick up a newer map file such as itrans.map if you copy it into the maps folder. When gumpad is run for the first time or when a new map is
loaded, gumpad will save either the built-in map or the new map to the file
C:\Documents and Settings\`<username>`\AppData\Roaming\GumPad\.gumpad.map on
Vista (or C:\Documents and Settings\`<username>`\Application
Data\GumPad\.gumpad.map on Windows XP) where `<username>` is the windows
username that you are logged into. If any customizations are made from the
Preferences->Map menu, they will be saved to the same .gumpad.map file. One
way of automatically picking up a newer itrans.map or other map file would be to copy it to this user specific .gumpad.map file. The reason for not automatically activating any map files is to preserve any customizations that were made and not saved.