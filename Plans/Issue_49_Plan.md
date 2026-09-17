# Plan: Issue 49 - Recognised phrase has no on-screen caption

## Exhaustive Outline
Currently, players cannot see the text of what the game thinks they said when using voice controls. This makes it difficult to know whether the game misunderstood the player or if the player made a mistake. The goal of this task is to display a brief on-screen text caption of the recognized phrase after every attempt. The text will have distinct visual styles depending on whether the attempt was a clean success, a misfire, or an invalid fizzle.

## Step by Step Execution Instructions

1.  **Locate User Interface:**
    Find the part of the game's screen display that handles showing text to the player.
2.  **Retrieve Recognized Phrase:**
    Modify the game's voice processing logic to pass along the exact, standardized text it heard.
3.  **Display the Text:**
    Connect the standardized text to the screen display so it appears as a caption right after the player attempts to use a voice command.
4.  **Differentiate Outcomes:**
    Change the appearance of the caption (like its color or formatting) based on the result of the attempt. Create unique looks for a successful cast, a misfire, and a silent fizzle.
5.  **Set Display Duration:**
    Make sure the caption only stays on the screen for a short moment before disappearing.

## Verification Steps
1.  Start the game and attempt a valid voice command, then check that the screen displays the correct text with the visual style for a clean cast.
2.  Attempt an incorrect voice command that triggers a misfire, then check that the screen displays the recognized text with the unique misfire visual style.
3.  Make a completely invalid sound or command, then check that the screen displays the recognized text with the visual style for a silent fizzle.
4.  Wait a few moments after any attempt to ensure the caption disappears from the screen automatically.

## Completion Checks
*   [ ] The recognized phrase is displayed on the screen briefly after every cast attempt.
*   [ ] The text shown is the standardized version that the game uses for processing.
*   [ ] The caption looks visually different for a clean cast, a misfire, and a silent fizzle.
