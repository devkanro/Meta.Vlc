param($installPath, $toolsPath, $package, $project)

$allowedReferences = @("Meta.Vlc.Wpf, Version=16.5.1.0, Culture=neutral, processorArchitecture=x86")

# Full assembly name is required
Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$projectCollection = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection

$allProjects = $projectCollection.GetLoadedProjects($project.Object.Project.FullName).GetEnumerator();

if($allProjects.MoveNext())
{
    $currentProject = $allProjects.Current

	Write-Host "Current Project $currentProject"

    foreach($Reference in $currentProject.GetItems('Reference') | ? {$allowedReferences -contains $_.Xml.Include })
    {
        $hintPath = $Reference.GetMetadataValue("HintPath")

        Write-Host "Matched againt $hintPath"

        #If it is x64 specific add condition (Include 'Any Cpu' as x64)
        if ($hintPath -match '.*\\(amd64|x64)\\.*\.dll$')
        {
            $Reference.Xml.Condition = "'TargetPlatform' != 'x86'"

            $condition = $Reference.Xml.Condition
            Write-Host "hintPath = $hintPath"
            Write-Host "condition = $condition"

            #Visual Studio doesnt allow the same reference twice (so try add friends)
            $matchingReferences = $currentProject.GetItems('Reference') | ? {($_.Xml.Include -eq $Reference.Xml.Include) -and ($_.GetMetadataValue("HintPath") -match ".*\\(x86)\\.*\.dll$")}

            #Add x86 Conditional
            Write-Host "Adding reference to $x86"

            $x86 = $hintPath -replace '(.*\\)(amd64|x64)(\\.*\.dll)$', '$1x86$3'
            $x86Path = Join-Path $installPath $x86

            $metaData = new-object "System.Collections.Generic.Dictionary``2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
            $metaData.Add("HintPath", $x86)
            $currentProject.AddItem('Reference', $Reference.Xml.Include, $metaData)

            $newReference = $currentProject.GetItems('Reference') | ? {($_.Xml.Include -eq $Reference.Xml.Include) -and ($_.GetMetadataValue("HintPath") -eq $x86)} | Select-Object -First 1

            $newReference.Xml.Condition = "'TargetPlatform' == 'x86'"
        }

        #If it is x86 specific add condition 
        if ($hintPath -match '.*\\x86\\.*\.dll$')
        {
            $Reference.Xml.Condition = "'TargetPlatform' == 'x86'"

            $condition = $Reference.Xml.Condition
            Write-Host "hintPath = $hintPath"
            Write-Host "condition = $condition"

            #Visual Studio doesnt allow the same reference twice (so try add friends)
            $matchingReferences = $currentProject.GetItems('Reference') | ? {($_.Xml.Include -eq $Reference.Xml.Include) -and ($_.GetMetadataValue("HintPath") -match ".*\\(amd64|x64)\\.*\.dll$")}

            #Add x64 Specific
            $x64 = $hintPath -replace '(.*\\)(x86)(\\.*\.dll)$', '$1x64$3'
            $x64Path = Join-Path $installPath $
            
            Write-Host "Adding reference to $x64"

            $metaData = new-object "System.Collections.Generic.Dictionary``2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
            $metaData.Add("HintPath", $x64)
            $currentProject.AddItem('Reference', $Reference.Xml.Include, $metaData)

            $newReference = $currentProject.GetItems('Reference') | ? {($_.Xml.Include -eq $Reference.Xml.Include) -and ($_.GetMetadataValue("HintPath") -eq $x64)} | Select-Object -First 1

            $newReference.Xml.Condition = "'TargetPlatform' != 'x86'"           
        }
    }
}