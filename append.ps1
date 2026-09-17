
$AppendText = "`r`n`r`n## Technical Constraints`r`nWhen executing this plan, you MUST strictly adhere to the project's C# and Unity conventions detailed in ``docs/UnityConvention.md``. This includes using ``m_camelCase`` for private fields, Allman braces, ``[SerializeField]`` for inspector exposure, BEM naming for UI, and the Single Responsibility Principle."
Get-ChildItem -Path Plans\Issue_*_Plan.md | ForEach-Object {
    Add-Content -Path $_.FullName -Value $AppendText
}

