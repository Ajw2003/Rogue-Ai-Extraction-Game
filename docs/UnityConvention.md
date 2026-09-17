# Unity 6 C# Clean Code & Style Guide for AI Agents

**System Instruction:** When generating, refactoring, or reviewing Unity C# code, you MUST strictly adhere to the following principles, naming conventions, formatting rules, and architectural patterns based on Unity's official "Use a C# style guide for clean and scalable game code" (Unity 6 Edition).

---

## 1. Core Architectural Principles
*   **KISS (Keep It Simple, Stupid):** Write the simplest code possible. Do not reinvent the wheel; utilize existing Unity Scripting APIs (e.g., `UnityEngine.Pool`) before writing custom solutions.
*   **YAGNI (You Aren't Gonna Need It):** Implement features *only* as they are needed right now. Do not add speculative features or code for future possibilities.
*   **Solve the Root Cause:** Do not write code around a problem (e.g., silencing a NullReferenceException with a quick `if-null` wrapper). Fix the actual source of the issue.
*   **DRY (Don't Repeat Yourself):** Avoid duplicate logic. Extract shared functionality into distinct, reusable methods.
*   **SRP (Single-Responsibility Principle):** Each class, module, or method must have one specific responsibility. Break large "God classes" (e.g., a monolith `Paddle` class) into smaller components (e.g., `PaddleData`, `PaddleInput`, `PaddleMovement`).

---

## 2. Naming Conventions
*   **Classes & Structs:** `PascalCase`. Must be nouns or noun phrases.
    *   *Rule:* A `MonoBehaviour` script name MUST perfectly match the file name.
*   **Interfaces:** `PascalCase` prefixed with a capital `I` and an adjective (e.g., `IKillable`, `IDamageable`).
*   **Namespaces:** `PascalCase` with no symbols or underscores. Match the logical/folder structure (e.g., `MyApplication.UI`).
*   **Enums:** `PascalCase` for both the enum type and its values. Use singular nouns (e.g., `WeaponType`). 
    *   *Exception:* Use plural nouns for bitwise enums marked with `[System.Flags]`.
*   **Public Fields & Properties:** `PascalCase` (e.g., `MaxHealth`).
*   **Private & Protected Variables:** `camelCase` with an `m_` prefix (e.g., `m_currentHealth`).
*   **Constants:** `PascalCase` with a `k_` prefix (e.g., `k_MaxItems`).
*   **Static Variables:** `camelCase` with an `s_` prefix (e.g., `s_myStaticField`).
*   **Local Variables & Parameters:** `camelCase` with no prefixes (e.g., `totalDamage`). Use meaningful, readable, and pronounceable nouns. Do not abbreviate unless used in standard math loops.
*   **Booleans:** Prefix with a verb that asks a question (e.g., `isDead`, `hasDamageMultiplier`).
*   **Methods:** `PascalCase`. Must start with a verb or verb phrase (e.g., `SetInitialPosition`). 
    *   *Rule:* Methods returning a boolean must ask a question (e.g., `IsGameOver()`).
*   **Events:** `PascalCase`. Use verb phrases indicating state changes (e.g., `DoorOpened`).
    *   *Publisher Rule:* Prefix the method that invokes the event with `On` (e.g., `OnDoorOpened()`).
    *   *Subscriber Rule:* Prefix event handler methods with the subject's name and an underscore (e.g., `GameEvents_DoorOpened()`).
    *   *Delegate:* Favor `System.Action` or `System.Action<T>`.

---

## 3. Formatting & Syntax
*   **Brace Style (Allman):** Opening curly braces `{` MUST be on a new line.
*   **Mandatory Braces:** NEVER omit braces for single-line `if`, `for`, or `while` statements. Always use `{ }` for clarity and safe refactoring.
*   **Indentation:** Use 4 spaces.
*   **Properties:** 
    *   Use expression-bodied members (`=>`) for single-line read-only properties (e.g., `public int MaxHealth => m_maxHealth;`).
    *   Use auto-implemented properties if no backing field is needed (e.g., `public int Health { get; private set; }`).
*   **Spacing:**
    *   Add a single space after commas in argument lists.
    *   No spaces after parentheses or before function names.
    *   Add a single space before flow control conditions (e.g., `if (x == y)`).
    *   Add a single space around comparison operators.
*   **Switch Statements:** Indent `case` statements from the `switch` block. ALWAYS include a `default` case.
*   **Var Keyword:** Use `var` only when the type is blatantly obvious from the context, especially to shorten lengthy instantiation lines or in `foreach` loops.
*   **Access Modifiers:** Omit redundant access modifiers (e.g., leave off `private` for fields if it simplifies reading, as `private` is the default).
*   **Variable Declarations:** One variable declaration per line.

---

## 4. Class & Method Organization
*   **Newspaper Metaphor (Top-Down):** Structure classes so they read like a newspaper article. High-level groundwork methods go first, followed by lower-level implementation details.
*   **Class Layout Order:**
    1. Fields (Variables)
    2. Properties
    3. Events / Delegates
    4. Unity MonoBehaviour Methods (Awake, Start, OnEnable, Update, etc.)
    5. Public Methods
    6. Private Methods
*   **Method Design:** Keep methods small. Avoid boolean flags as arguments to toggle behaviors (create two separate methods instead, e.g., `GetAngleInDegrees()` and `GetAngleInRadians()`). Avoid side effects.
*   **Extension Methods:** Place them in static classes named intuitively (e.g., `TransformExtensions`). Use the `this` keyword on the first argument.

---

## 5. Unity-Specific Implementations
*   **Serialization:** 
    *   NEVER make a variable `public` just to expose it in the Inspector. Use `[SerializeField] private Type m_VariableName;`.
    *   Use `[Range(min, max)]` for numeric fields to provide slider constraints.
    *   Group related data into a `public struct` or `class` marked with `[Serializable]` to organize the Inspector.
*   **Tooltips:** Use `[Tooltip("Description")]` on serialized fields instead of writing code comments.

---

## 6. UI Toolkit (UXML/USS) Naming
*   **BEM Convention:** Use Block Element Modifier convention (`block-name__element-name--modifier-name`) for UI class names.
*   **Casing:** Use `kebab-case` (hyphen delimited) for all USS classes.
*   **Semantics:** Ensure names are semantic (e.g., `button--quit`), NOT presentational (e.g., `button--red`). Do not use element types in the class name unless strictly necessary.

---

## 7. Comments
*   **Why, Not What:** Comments must explain *why* a decision was made or *why* logic works a certain way. If you have to explain *what* the code does, your naming and structure are bad. Refactor instead.
*   **XML Summaries:** Use `/// <summary>` tags above public methods/classes to drive IntelliSense.
*   **Cleanup:** NEVER leave commented-out code blocks in production. Rely on version control. Clean up `// TODO` comments aggressively.
```eof

This `.md` file is strictly formatted to serve as a high-level system prompt or context document for your AI agents, giving them exact constraints on naming schemes, logic structuring, and Unity-specific API best practices.