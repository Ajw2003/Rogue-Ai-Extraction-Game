# Plan: Issue 47 - No loudness meter - the whisper/normal/shout dial the pitch calls "the whole trick" is invisible

## Exhaustive Outline
Currently, the game changes spell strength and noise based on how loudly the player speaks, but there is nothing on the screen to show the player their current voice volume while casting. The goal of this task is to add a visible volume indicator to the screen during spell casting so players can easily see if they are whispering, speaking normally, or shouting. This will help them understand why a spell might have failed for being too loud or succeeded weakly for being too quiet.

## Step by Step Execution Instructions

1.  **Create Volume Indicator:**
    Build a visual element on the game screen that can show volume levels from whisper to shout.
2.  **Link Indicator to Voice Volume:**
    Connect this new visual element to the system that measures the player's voice volume while the cast key is held down.
3.  **Update Indicator Continuously:**
    Make sure the visual indicator changes instantly while the cast key is held to show the current volume level.
4.  **Clarify Volume Zones:**
    Adjust the design of the indicator so players can clearly see the boundaries for whispering, normal speaking, and shouting.

## Verification Steps
1.  Start the game and hold the cast key to prepare a spell.
2.  Whisper into your microphone and confirm the indicator shows a low volume.
3.  Speak normally and confirm the indicator shows a medium volume.
4.  Speak loudly and confirm the indicator shows a high volume.
5.  Cast a spell that requires a quiet voice while speaking loudly to ensure the indicator clearly shows the volume was too high when the spell fails.

## Completion Checks
*   [ ] A visual indicator appears on screen while holding the cast key.
*   [ ] The indicator tracks and displays voice volume instantly as it changes.
*   [ ] It is easy to tell from the indicator if the volume is at a whisper, normal, or shout level.
