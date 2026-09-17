# Plan: Issue 16 - Main Menu Art and Styling

## Exhaustive Outline
Currently, the main menu and the Lair screen are extremely basic. They use solid flat colors and default button styles, which makes the game feel unfinished and sets no mood or atmosphere. The goal of this task is to give these menus a proper visual identity. We will add a background image or a 3D scene behind the menu, style the buttons and text to fit the game's theme, and ensure the Main Menu and Lair screens look consistent with each other.

## Step by Step Execution Instructions

1.  **Add Menu Backgrounds:**
    Create or acquire appropriate artwork for the main menu background. This could be a static 2D image, a looping video, or a dedicated 3D scene (like a dimly lit room). Add this background to the `MainMenuScreen` and `LairScreen` layouts.

2.  **Define UI Theme Elements:**
    Update the `UITheme` settings or create new UI prefabs. Choose a font that fits the fantasy or dark tone of the game. Design custom textures or styling rules for the buttons, such as hover effects, borders, and color palettes.

3.  **Apply Styling to Menus:**
    Open the `MainMenuScreen.cs` and `LairScreen.cs` scripts or their corresponding visual layouts. Update the title text, the version label, and all buttons to use the newly defined fonts and styles instead of the factory defaults.

4.  **Ensure Consistency:**
    Compare the Main Menu screen to the Lair screen. Make sure the buttons, fonts, and background themes match perfectly so the transition between them feels smooth.

## Verification Steps
1.  Launch the game so it opens on the Main Menu.
2.  Verify the background is no longer a solid flat color.
3.  Check the game title, buttons, and version text. Verify they use the new custom font and style.
4.  Navigate from the Main Menu into the Lair screen.
5.  Verify the Lair screen uses the same cohesive art style and background treatment as the Main Menu.

## Completion Checks
*   [ ] The main menu has a deliberate artistic background (2D or 3D).
*   [ ] The UI fonts, text, and buttons use custom styling instead of default boxes.
*   [ ] The Lair screen has been updated to use the exact same visual style.
*   [ ] The overall look establishes a mood that fits the game's theme.


## Technical Constraints
When executing this plan, you MUST read and strictly adhere to ALL principles and conventions detailed in `docs/UnityConvention.md`. 
You cannot pick and choose which rules to enforce; every single rule applies.
Specifically, you must follow:
- Core Architectural Principles: KISS, YAGNI, Solve the Root Cause, DRY, and SRP.
- All Naming Conventions (e.g., `m_camelCase` for privates, `s_camelCase` for statics, `PascalCase` for methods/properties).
- All Formatting & Syntax rules (e.g., Allman braces, mandatory braces, 4-space indentation).
- Class & Method Organization (Newspaper metaphor, correct layout order).
- Unity-Specific Implementations (e.g., `[SerializeField]` instead of public, `[Tooltip]` instead of comments).
- UI Toolkit (UXML/USS) Naming (BEM convention, kebab-case).
- Commenting rules (Explain 'Why', not 'What').
