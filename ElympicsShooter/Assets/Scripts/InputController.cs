using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

[RequireComponent(typeof(InputProvider))]
public class InputController : ElympicsMonoBehaviour, IInputHandler, IInitializable
{
	[SerializeField] private MovementController movementController = null;
	[SerializeField] private LoadoutController loadoutController = null;
	[SerializeField] private PlayerData playerData = null;

	private InputProvider inputProvider = null;

	public void Initialize()
	{
		this.inputProvider = GetComponent<InputProvider>();
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
		inputWriter.Write(inputProvider.Movement.x);
		inputWriter.Write(inputProvider.Movement.y);

		inputWriter.Write(inputProvider.Jump);

		inputWriter.Write(inputProvider.WeaponPrimaryAction);
	}

	public void ApplyInput(ElympicsPlayer player, IInputReader inputDeserializer)
	{
		inputDeserializer.Read(out float forwardMovement);
		inputDeserializer.Read(out float rightMovement);

		inputDeserializer.Read(out bool jump);
		inputDeserializer.Read(out bool weaponPrimaryAction);

		if (playerData.PlayerId != (int)player)
			return;

		ProcessMovement(forwardMovement, rightMovement, jump);
		ProcessLoadoutActions(weaponPrimaryAction);
	}

	private void ProcessLoadoutActions(bool weaponPrimaryAction)
	{
		loadoutController.ProcessWeaponActions(weaponPrimaryAction);
	}

	private void ProcessMovement(float forwardMovement, float rightMovement, bool jump)
	{
		movementController.ProcessMovement(forwardMovement, rightMovement, jump);
	}
}
