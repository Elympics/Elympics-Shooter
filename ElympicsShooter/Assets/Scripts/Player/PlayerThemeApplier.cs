using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class PlayerThemeApplier : MonoBehaviour
{
	[SerializeField] private SkinnedMeshRenderer[] themeBasedRenderers = null;

	private void Awake()
	{
		var playerData = GetComponent<PlayerData>();

		ApplyTheme(playerData.ThemeMaterial);
	}

	private void ApplyTheme(Material themeMaterial)
	{
		foreach (SkinnedMeshRenderer themeBasedRenderer in themeBasedRenderers)
			themeBasedRenderer.material = themeMaterial;
	}
}
