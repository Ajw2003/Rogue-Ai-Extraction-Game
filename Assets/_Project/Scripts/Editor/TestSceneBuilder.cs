using Code.Scripts.EventSystems;
using Player;
using StateMachine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Development convenience only, not shipped gameplay. See docs/systems/core.md - Dev tooling.
public static class TestSceneBuilder
{
    // Judgement calls - tune here rather than hunting through the generated scene.
    // At radius 25 the horizon sat about two body-lengths away and a lap took under half a minute,
    // which made the curvature read as a bug rather than a planet. 80 walks a lap in roughly 80
    // seconds at PlayerWalkSpeed.
    private const float PlanetRadius = 80f;
    private const float PlanetInfluenceRadius = 200f;
    private const float PlanetGravityStrength = 9.81f;
    private const float PlayerCapsuleHeight = 2f;
    private const float PlayerCapsuleRadius = 0.5f;
    private const float PlayerWalkSpeed = 6f;
    private const float PlayerJumpForce = 7f;
    private const float PlayerGroundCheckRadius = 0.3f;
    private const float PlayerGroundCheckDistance = 1.6f;
    private const float PlayerGroundedHeight = 1f;
    private const float ItemSize = 0.75f;

    private const string ScenePath = "Assets/_Project/Scenes/TestScene.unity";
    private const string PlayerPrefabPath = "Assets/_Project/Prefabs/Player.prefab";

    [MenuItem("Tools/RogueAi/Build Test Scene")]
    public static void BuildTestScene()
    {
        // A fresh scene built from scratch every run, saved over the same path, is what keeps
        // this idempotent - there is never a second planet to accumulate.
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        BuildSceneLight();
        BuildManagers();

        GameObject planet = BuildPlanet();
        GameObject playerPrefab = BuildPlayerPrefab();
        GameObject player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab, scene);
        BuildTestItem(planet.transform);

        player.transform.position = new Vector3(0f, PlanetRadius + PlayerGroundedHeight + PlayerCapsuleHeight * 0.5f, 0f);

        EnsureSceneFolderExists();
        EditorSceneManager.SaveScene(scene, ScenePath);

        Debug.Log($"RogueAi: test scene built at {ScenePath} with player prefab at {PlayerPrefabPath}.");
    }

    // NewSceneSetup.EmptyScene omits the default light, so without this the planet renders unlit
    // and the scene looks broken before any gameplay is even exercised.
    private static void BuildSceneLight()
    {
        GameObject lightObject = new GameObject("DirectionalLight");
        Light directionalLight = lightObject.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    // ItemManager drives the grab/carry/throw input and is a scene-owned singleton, so without an
    // instance in the scene ItemManager.Instance stays null and clicking an item does nothing.
    // EventManager backs the input controller's re-enable path for the same reason.
    private static void BuildManagers()
    {
        new GameObject("EventManager").AddComponent<EventManager>();
        new GameObject("ItemManager").AddComponent<ItemManager>();
    }

    private static GameObject BuildPlanet()
    {
        GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        planet.name = "TestPlanet";
        planet.transform.position = Vector3.zero;
        // The primitive sphere mesh has a radius of 0.5, so scale it up to PlanetRadius.
        planet.transform.localScale = Vector3.one * (PlanetRadius * 2f);

        GravitySource gravitySource = planet.AddComponent<GravitySource>();
        gravitySource.type = GravitySource.GravityType.Spherical;
        gravitySource.gravityStrength = PlanetGravityStrength;
        gravitySource.influenceRadius = PlanetInfluenceRadius;

        return planet;
    }

    private static GameObject BuildPlayerPrefab()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";

        CapsuleCollider capsule = player.AddComponent<CapsuleCollider>();
        capsule.height = PlayerCapsuleHeight;
        capsule.radius = PlayerCapsuleRadius;

        // Adding PlayerStateMachine pulls in Rigidbody and GravityReceiver via their
        // [RequireComponent] chain (PlayerStateMachine -> GravityReceiver -> Rigidbody).
        PlayerStateMachine stateMachine = player.AddComponent<PlayerStateMachine>();
        stateMachine.walkSpeed = PlayerWalkSpeed;
        stateMachine.JumpForce = PlayerJumpForce;

        SetGroundCheckFields(stateMachine);

        // Without this nothing ever calls Move/Jump/Look on the state machine, so the player sits
        // in PlayerIdleState and reads as completely unresponsive.
        player.AddComponent<PlayerInputController>();

        Transform cameraPivot = new GameObject("CameraPivot").transform;
        cameraPivot.SetParent(player.transform, false);
        cameraPivot.localPosition = new Vector3(0f, PlayerCapsuleHeight * 0.5f, 0f);
        stateMachine.CameraTransform = cameraPivot;

        GameObject cameraObject = new GameObject("PlayerCamera");
        cameraObject.tag = "MainCamera";
        cameraObject.AddComponent<Camera>();
        cameraObject.transform.SetParent(cameraPivot, false);
        cameraObject.transform.localPosition = new Vector3(0f, 0.5f, -4f);

        EnsurePrefabFolderExists();
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(player, PlayerPrefabPath);
        Object.DestroyImmediate(player);
        return prefab;
    }

    // _groundLayer, _groundCheckRadius, etc. are private [SerializeField] fields per the project's
    // style rules, so they need SerializedObject rather than a direct field write.
    private static void SetGroundCheckFields(PlayerStateMachine stateMachine)
    {
        SerializedObject serialized = new SerializedObject(stateMachine);
        serialized.FindProperty("_groundCheckRadius").floatValue = PlayerGroundCheckRadius;
        serialized.FindProperty("_groundCheckDistance").floatValue = PlayerGroundCheckDistance;
        serialized.FindProperty("_groundedHeight").floatValue = PlayerGroundedHeight;
        serialized.FindProperty("_groundLayer").intValue = LayerMask.GetMask("Default");
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void BuildTestItem(Transform planetTransform)
    {
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        item.name = "TestItem";
        item.transform.localScale = Vector3.one * ItemSize;
        item.transform.position = new Vector3(3f, PlanetRadius + ItemSize, 0f);

        // Item's [RequireComponent] chain pulls in Rigidbody and GravityReceiver; CreatePrimitive
        // already gave the cube a BoxCollider for the grab/throw physics to act on.
        item.AddComponent<Item>();
    }

    private static void EnsureSceneFolderExists()
    {
        EnsureFolderExists("Assets/_Project/Scenes");
    }

    private static void EnsurePrefabFolderExists()
    {
        EnsureFolderExists("Assets/_Project/Prefabs");
    }

    private static void EnsureFolderExists(string folderPath)
    {
        if (folderPath == "Assets" || AssetDatabase.IsValidFolder(folderPath)) return;

        string parent = System.IO.Path.GetDirectoryName(folderPath).Replace('\\', '/');
        string folderName = System.IO.Path.GetFileName(folderPath);
        EnsureFolderExists(parent);
        AssetDatabase.CreateFolder(parent, folderName);
    }
}
