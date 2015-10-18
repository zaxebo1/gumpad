GumPad ships with gumc.exe which is a command line interface to the GumPad transliteration engine.

_Note_

**gumc** was added in version 2.0.0.3

## Usage ##

usage: gumc -i infile -o outfile [ -lang te|hi|ta|ka|ma|mr|or|be|gu|gr|en|
ex ] [-m mapfile ](.md) [-f input-filter ](.md) [ -e ascii|utf8|utf16|utf16be|utf32
]


<table border='0'>

<blockquote><tr>
<blockquote><td>-i infile</td>
<td>Specify input file name</td>
</blockquote></tr></blockquote>

<blockquote><tr>
<blockquote><td>-o outfile</td>
<td>Specify output file name</td>
</blockquote></tr></blockquote>

<blockquote><tr>
<blockquote><td>lang lang</td>
<td>Specify language to convert to</td>
</blockquote></tr></blockquote>

<blockquote><tr>
<blockquote><td></td>
<td>hi = Devanagari</td>
</blockquote></tr>
<tr>
<blockquote><td></td>
<td>te = Telugu</td>
</blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>ta = Tamil</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>ka = Kannada</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>ma = Malayalam</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>mr = Marathi</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>or = Oriya</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>be = Bengali</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>gu = Gujarati</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>gr = Gurmukhi</td>
</blockquote></blockquote></tr>
<tr>
<blockquote><td></td>
<blockquote><td>en = English</td>
</blockquote></blockquote></tr></blockquote>

<blockquote><tr>
<blockquote><td>-m mapfile</td>
<td>Specify conversion map file to use for conversion</td>
</blockquote></tr></blockquote>

<blockquote><tr>
<blockquote><td>-f input-filter</td>
<td>Specify an Input filter program to transform the input file before converrsion. E.g., use detex to remove latex markup. <i>Use with caution. I haven't tested this much</i></td>
</blockquote></tr></blockquote>

<blockquote><tr>
<blockquote><td>--e encoding</td>
<td>Input file encoding.</td>
</blockquote></tr></blockquote>

</table>