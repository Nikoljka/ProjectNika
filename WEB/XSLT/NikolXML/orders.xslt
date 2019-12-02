<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <table border="1">
      <tr>
        <th>Id</th>
        <th>Count</th>
        <th>Price</th>
        <th>Customer</th>
        <th>Date</th>
      </tr>
      <xsl:for-each select="orders/order">
        <tr>
          <th>
            <xsl:value-of select="id"/>
          </th>
          <th>
            <xsl:value-of select="count"/>
          </th>
          <th>
            <xsl:value-of select="price"/>
          </th>
          <th>
            <xsl:value-of select="customer"/>
          </th>
          <th>
            <xsl:value-of select="date"/>
          </th>
        </tr>
      </xsl:for-each>
    </table>
    <br/>
    <br/>
    <br/>
    First name of second order customer: <xsl:value-of select="substring-before(orders/order[2]/customer, ' ')"/>
    <br/>
    Last name of second order customer: <xsl:value-of select="substring-after(orders/order[2]/customer, ' ')"/>
    <br/>
    Name of the last order customer: <xsl:value-of select="/orders/order[last()]/customer"/>
  </xsl:template>
</xsl:stylesheet>