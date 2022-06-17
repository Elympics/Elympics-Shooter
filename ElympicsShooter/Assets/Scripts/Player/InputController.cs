using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

[RequireComponent(typeof(InputProvider))]
public class InputController : ElympicsMonoBehaviour, IInputHandler, IInitializable
{
	[SerializeField] private MovementController movementController = null;
	[SerializeField] private ViewController viewController = null;
	[SerializeField] private LoadoutController loadoutController = null;
	[SerializeField] private HUDController hudController = null;
	[SerializeField] private PlayerData playerData = null;
	[SerializeField] private PlayerScoresManager playerScoresManager = null;

	private InputProvider inputProvider = null;
	private bool canProcessInputs = true;

	public void Initialize()
	{
		this.inputProvider = GetComponent<InputProvider>();

		playerScoresManager.GameEnded.ValueChanged += OnGameEndedStatusChanged;
	}

	private void OnGameEndedStatusChanged(bool gameEndedLastValue, bool gameEndedNewValue)
	{
		canProcessInputs &= !gameEndedNewValue;
	}

	public void GetInputForBot(IInputWriter inputSerializer)
	{
		SerializeInput(inputSerializer);
	}

	public void GetInputForClient(IInputWriter inputSerializer)
	{
		SerializeInput(inputSerializer);
	}

	private void SerializeInput(IInputWriter inputWriter)
	{
		//movement
		inputWriter.Write(inputProvider.Movement.x);
		inputWriter.Write(inputProvider.Movement.y);

		//mouse
		inputWriter.Write(inputProvider.MouseAxis.x);
		inputWriter.Write(inputProvider.MouseAxis.y);
		inputWriter.Write(inputProvider.MouseAxis.z);

		//action buttons
		inputWriter.Write(inputProvider.Jump);
		inputWriter.Write(inputProvider.WeaponPrimaryAction);
		inputWriter.Write(inputProvider.ShowScoreboard);
		inputWriter.Write(inputProvider.WeaponSlot);
	}

	public void ApplyInput(ElympicsPlayer player, IInputReader inputDeserializer)
	{
		inputDeserializer.Read(out float forwardMovement);
		inputDeserializer.Read(out float rightMovement);

		inputDeserializer.Read(out float xRotation);
		inputDeserializer.Read(out float yRotation);
		inputDeserializer.Read(out float zRotation);

		inputDeserializer.Read(out bool jump);
		inputDeserializer.Read(out bool weaponPrimaryAction);
		inputDeserializer.Read(out bool showScoreboard);
		inputDeserializer.Read(out int weaponSlot);

		if ((playerData.PlayerId != (int)player) || !canProcessInputs)
			return;

		ProcessMovement(forwardMovement, rightMovement, jump);

		ProcessMouse(Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation)));

		ProcessLoadoutActions(weaponPrimaryAction, weaponSlot);

		ProcessHUDActions(showScoreboard);
	}

	private void ProcessHUDActions(bool showScoreboard)
	{
		hudController.ProcessHUDActions(showScoreboard);
	}

	private void ProcessMouse(Quaternion mouseRotation)
	{
		viewController.ProcessView(mouseRotation);
	}

	private void ProcessLoadoutActions(bool weaponPrimaryAction, int weaponSlot)
	{
		loadoutController.ProcessLoadoutActions(weaponPrimaryAction, weaponSlot);
	}

	private void ProcessMovement(float forwardMovement, float rightMovement, bool jump)
	{
		movementController.ProcessMovement(forwardMovement, rightMovement, jump);
	}
}
