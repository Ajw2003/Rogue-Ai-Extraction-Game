Vosk small English model target folder
======================================

This folder must contain the extracted contents of:
  vosk-model-small-en-us-0.15  (~40MB, from https://alphacephei.com/vosk/models/)

The model itself is NOT committed to git (too large). To install it:

  1. In the Unity editor, run the menu:
       Plunderspell > Voice > Download Vosk Small Model
     (downloads + extracts automatically into this folder)

  OR manually:
  2. Download https://alphacephei.com/vosk/models/vosk-model-small-en-us-0.15.zip
  3. Extract it and copy the *contents* of the vosk-model-small-en-us-0.15/
     directory directly into this folder, so you end up with:
       small-en-us/am/
       small-en-us/conf/
       small-en-us/graph/
       small-en-us/ivector/
       ...

At runtime VoskVoiceInputService loads the model from:
  Application.streamingAssetsPath + "/VoskModels/small-en-us/"

If the model is missing, voice casting logs a warning and stays silent
(the game does not crash).
