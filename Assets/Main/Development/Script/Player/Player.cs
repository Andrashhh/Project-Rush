using UnityEngine;
using UnityEngine.InputSystem;

namespace Root
{
    public class Player : MonoBehaviour
    {
        public static string PlayerTag = "Player";

        [SerializeField] PlayerCharacter playerCharacter;
        [SerializeField] PlayerCamera playerCamera;
        [SerializeField] CameraSpring cameraSpring;
        [SerializeField] CameraLean cameraLean;
        [SerializeField] AbilitySlots playerAbilities;

        InputSys inputActions;

        CameraInput cameraInput;
        CharacterInput characterInput;
        AbilityInput abilityInput;

        CharacterState state;

        Transform cameraTargetTransform;
        float deltaTime;

        void Initialize() {
            playerCharacter.Initialize();
            playerAbilities.Initialize();
        }

        void Start() {
            Cursor.lockState = CursorLockMode.Locked;

            inputActions = new InputSys();
            inputActions.Enable();


            Initialize();
        }

        void OnDestroy() {
            inputActions.Dispose();
        }

        void Update() {
            UpdateVariables();

            UpdateInputs(inputActions.Player);
            UpdatePlayerCharacter(characterInput, deltaTime);
            UpdatePlayerCamera(cameraInput, characterInput, state);
            UpdateAbility(abilityInput);

#if UNITY_EDITOR
            if(Keyboard.current.tKey.wasPressedThisFrame) {
                var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                if(Physics.Raycast(ray, out var hit)) {
                    Teleport(hit.point);
                }
            }
#endif
        }

        void LateUpdate() {
            LateUpdatePlayerCamera(cameraTargetTransform, deltaTime, state);
        }

        void UpdateVariables() {
            deltaTime = Time.deltaTime;
            cameraTargetTransform = playerCharacter.GetCameraTarget();
            state = playerCharacter.GetState();
        }

        void UpdateInputs(InputSys.PlayerActions input) {
            cameraInput = new CameraInput {
                Look = input.Look.ReadValue<Vector2>()
            };

            characterInput = new CharacterInput {
                Rotation = playerCamera.transform.rotation,
                Move = input.Move.ReadValue<Vector2>(),
                Sprint = input.Sprint.IsPressed(),
                Jump = input.Jump.WasPressedThisFrame(),
                //Crouch      = input.Crouch.WasPressedThisFrame() ? CrouchInput.Toggle : CrouchInput.None <- this is toggle input
                Crouch = input.Crouch.IsPressed() ? CrouchInput.Toggle : CrouchInput.None
            };

            abilityInput = new AbilityInput {
                Slot1 = input.Ability1.WasPressedThisFrame(),
                Slot2 = input.Ability2.WasPressedThisFrame(),
                Slot3 = input.Ability3.WasPressedThisFrame(),
                Slot4 = input.Ability4.WasPressedThisFrame(),
            };
        }

        void UpdateAbility(AbilityInput abilityInput) {
            playerAbilities.UpdateMaxCD(Slot.First, 10f);
            playerAbilities.UpdateMaxCD(Slot.Second, 4f);
            playerAbilities.UpdateMaxCD(Slot.Third, 30f);
            playerAbilities.UpdateMaxCD(Slot.Fourth, 120f);

            playerAbilities.UpdateSlots(abilityInput);
        }

        void UpdatePlayerCharacter(CharacterInput characterInput, float deltaTime) {
            playerCharacter.UpdateInput(characterInput);
            playerCharacter.UpdateBody(deltaTime);
        }

        void UpdatePlayerCamera(CameraInput cameraInput, CharacterInput characterInput, CharacterState state) {
            playerCamera.UpdateRotation(cameraInput);
            playerCamera.UpdateFov(state, characterInput);
        }

        void LateUpdatePlayerCamera(Transform cameraTargetTransform, float deltaTime, CharacterState state) {
            cameraSpring.UpdateSpring(deltaTime, cameraTargetTransform.up);
            cameraLean.UpdateLean(deltaTime, state, state.Acceleration, state.Velocity, cameraTargetTransform.up);
            playerCamera.UpdatePosition(cameraTargetTransform);
        }

        public void Teleport(Vector3 pos) {
            playerCharacter.SetPosition(pos);
        }

    }
}
