<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet
 xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="2.0" >
  <xsl:template match="TransliterationMap">
    <html>
      <head>
        <title>
          Transliteration Map
        </title>
        <LINK REL="StyleSheet" HREF="gumpad.css" TYPE="text/css" MEDIA="screen"></LINK>
      </head>
      <body>
        <table border="1">
          <tr>
            <th>Language</th>
            <th>Character</th>
            <th>Input Sequence</th>
            <th>Unicode Output</th>
            <th>Extended Latin Output</th>
          </tr>
          <xsl:for-each select="mapping">
            <tr>
              <td>
                <xsl:value-of select="language"/>
              </td>
              <td>
                <xsl:value-of select="character-name"/>
              </td>
              <td>
                <xsl:for-each select="input-sequence">
                  <xsl:value-of select="." />
                  <br/>
                </xsl:for-each>
              </td>
              <td>
                <xsl:value-of select="unicode-output-sequence"/>
              </td>
              <td>
                <xsl:value-of select="extended-latin-output-sequence"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
