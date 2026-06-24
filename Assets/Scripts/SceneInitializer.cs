using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public int targetScore = 5;
    public Vector3 playerStart = new Vector3(0f, 1f, 0f);
    public Color playerColor = Color.cyan;
    public Color groundColor = Color.green;
    public Color collectibleColor = Color.yellow;
    public Color hazardColor = Color.red;
    public Color enemyColor = Color.magenta;
    public Color powerUpColor = Color.blue;

    private Vector3[] collectiblePositions = new Vector3[]
    {
        new Vector3(2f, 0.5f, 2f),
        new Vector3(-2f, 0.5f, 2f),
        new Vector3(2f, 0.5f, -2f),
        new Vector3(-2f, 0.5f, -2f),
        new Vector3(0f, 0.5f, 3f)
    };

    private GameObject player;

    private void Awake()
    {
        CreateUIManager();
        CreateGameManager();
        CreateGround();
        CreateHazards();
        CreateMovingPlatform();
        CreatePlayer();
        CreateEnemyPatrol();
        CreateCollectibles();
        CreatePowerUps();
        CreateLight();
        CreateCamera();
    }

    private void CreateUIManager()
    {
        GameObject uiObject = new GameObject("UIManager");
        uiObject.AddComponent<UIManager>();
    }

    private void CreateGameManager()
    {
        GameObject managerObject = new GameObject("GameManager");
        GameManager gameManager = managerObject.AddComponent<GameManager>();
        gameManager.targetScore = targetScore;
    }

    private void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0f, -0.5f, 0f);
        ground.transform.localScale = new Vector3(12f, 1f, 12f);

        Renderer renderer = ground.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateMaterial("Ground Material", groundColor);
        ground.GetComponent<Collider>().isTrigger = false;
    }

    private void CreateHazards()
    {
        CreateHazard(new Vector3(3f, 0.5f, 0f), new Vector3(1.5f, 1f, 1.5f), hazardColor, 1);
        CreateHazard(new Vector3(-3f, 0.5f, 2.5f), new Vector3(1.5f, 1f, 1.5f), hazardColor, 2);
    }

    private void CreateHazard(Vector3 position, Vector3 scale, Color color, int damage)
    {
        GameObject hazard = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hazard.name = "Hazard";
        hazard.transform.position = position;
        hazard.transform.localScale = scale;

        Renderer renderer = hazard.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateMaterial("Hazard Material", color);

        Collider collider = hazard.GetComponent<Collider>();
        collider.isTrigger = true;

        Hazard hazardComponent = hazard.AddComponent<Hazard>();
        hazardComponent.damage = damage;
    }

    private void CreateMovingPlatform()
    {
        GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.name = "Moving Platform";
        platform.transform.position = new Vector3(0f, 0.75f, -1.5f);
        platform.transform.localScale = new Vector3(4f, 0.3f, 2f);

        Renderer renderer = platform.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateMaterial("Platform Material", Color.gray);

        platform.AddComponent<MovingPlatform>().offset = new Vector3(6f, 0f, 0f);
    }

    private void CreatePlayer()
    {
        player = GameObject.CreatePrimitive(PrimitiveType.Cube);
        player.name = "Player";
        player.transform.position = playerStart;
        player.transform.localScale = Vector3.one;
        player.tag = "Player";

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        PlayerController playerController = player.AddComponent<PlayerController>();
        playerController.moveSpeed = 5f;
        playerController.jumpForce = 5f;

        Renderer renderer = player.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateMaterial("Player Material", playerColor);
    }

    private void CreateEnemyPatrol()
    {
        GameObject pointA = CreatePoint("Patrol Point A", new Vector3(-4f, 1f, 4f));
        GameObject pointB = CreatePoint("Patrol Point B", new Vector3(4f, 1f, 4f));

        GameObject enemyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemyObject.name = "Enemy";
        enemyObject.transform.position = pointA.transform.position;
        enemyObject.transform.localScale = new Vector3(1f, 1.5f, 1f);
        enemyObject.tag = "Enemy";

        Renderer renderer = enemyObject.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateMaterial("Enemy Material", enemyColor);

        EnemyPatrol enemyPatrol = enemyObject.AddComponent<EnemyPatrol>();
        enemyPatrol.pointA = pointA.transform;
        enemyPatrol.pointB = pointB.transform;
        enemyPatrol.patrolSpeed = 2f;
        enemyPatrol.chaseSpeed = 3.5f;
        enemyPatrol.chaseRange = 6f;
        enemyPatrol.damage = 1;
    }

    private GameObject CreatePoint(string name, Vector3 position)
    {
        GameObject point = new GameObject(name);
        point.transform.position = position;
        return point;
    }

    private void CreateCollectibles()
    {
        for (int i = 0; i < collectiblePositions.Length; i++)
        {
            GameObject collectibleObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            collectibleObject.name = $"Collectible {i + 1}";
            collectibleObject.transform.position = collectiblePositions[i];
            collectibleObject.transform.localScale = Vector3.one * 0.7f;

            Renderer renderer = collectibleObject.GetComponent<Renderer>();
            renderer.sharedMaterial = CreateMaterial($"Collectible Material {i}", collectibleColor);

            Collider collider = collectibleObject.GetComponent<Collider>();
            collider.isTrigger = true;

            Collectible collectible = collectibleObject.AddComponent<Collectible>();
            collectible.value = 1;
        }
    }

    private void CreatePowerUps()
    {
        CreatePowerUp(new Vector3(0f, 0.5f, -3.5f), PowerUp.PowerUpType.SpeedBoost, 5f, 2f);
        CreatePowerUp(new Vector3(3.5f, 0.5f, 3f), PowerUp.PowerUpType.Shield, 6f, 0f);
        CreatePowerUp(new Vector3(-3.5f, 0.5f, -3f), PowerUp.PowerUpType.DoubleJump, 6f, 0f);
    }

    private void CreatePowerUp(Vector3 position, PowerUp.PowerUpType type, float duration, float value)
    {
        GameObject powerUpObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        powerUpObject.name = type.ToString() + " PowerUp";
        powerUpObject.transform.position = position;
        powerUpObject.transform.localScale = Vector3.one * 0.8f;

        Renderer renderer = powerUpObject.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateMaterial(type.ToString() + " Material", powerUpColor);

        Collider collider = powerUpObject.GetComponent<Collider>();
        collider.isTrigger = true;

        PowerUp powerUp = powerUpObject.AddComponent<PowerUp>();
        powerUp.powerUpType = type;
        powerUp.duration = duration;
        powerUp.value = value;
    }

    private void CreateLight()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    private void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.tag = "MainCamera";
        cameraObject.AddComponent<AudioListener>();

        cameraObject.transform.position = new Vector3(0f, 6f, -10f);
        cameraObject.transform.LookAt(player.transform.position + Vector3.up * 0.5f);
    }

    private Material CreateMaterial(string name, Color color)
    {
        Material material = new Material(Shader.Find("Standard"));
        material.name = name;
        material.color = color;
        return material;
    }
}
