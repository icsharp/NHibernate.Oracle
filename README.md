# NHibernate.Oracle
This repo is forked from nhibernate/NHibernate.Oracle, and update to latest Official Oracle ODP.NET, Managed Driver.
### Install Package
```
Install-Package NHibernate.Driver.OracleManagedDataAccess
```
### Configuration samples
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <configSections>
        <section name="hibernate-configuration" type="NHibernate.Cfg.ConfigurationSectionHandler, NHibernate"/>
        <section name="oracle.manageddataaccess.client" type="OracleInternal.Common.ODPMSectionHandler, Oracle.ManagedDataAccess, Version=4.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342"/>
    </configSections>
    <hibernate-configuration xmlns="urn:nhibernate-configuration-2.2">
        <session-factory>
            <property name="connection.provider">NHibernate.Connection.DriverConnectionProvider, NHibernate</property>
            <property name="dialect">NHibernate.Dialect.Oracle10gDialect</property>
            <property name="connection.driver_class">NHibernate.Driver.OracleManaged.OracleManagedDataClientDriver,NHibernate.Driver.OracleManaged</property>
            <property name="show_sql">true</property>
            <property name="command_timeout">60</property>
            <property name="adonet.batch_size">100</property>
            <property name="connection.connection_string_name">MyConnectionString</property>
            <property name="proxyfactory.factory_class"> NHibernate.Bytecode.DefaultProxyFactoryFactory, NHibernate</property>
            <mapping assembly="NHibernate.ManagedDataAccess.Samples"/>
        </session-factory>
    </hibernate-configuration>
    <connectionStrings>
        <add name="MyConnectionString" connectionString="data source=mytest;user id=***;password=***"/>
    </connectionStrings>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1"/>
    </startup>
    <runtime>
        <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
            <dependentAssembly>
                <publisherPolicy apply="no"/>
                <assemblyIdentity name="Oracle.ManagedDataAccess" publicKeyToken="89b483f429c47342" culture="neutral"/>
                <bindingRedirect oldVersion="4.121.0.0 - 4.65535.65535.65535" newVersion="4.121.2.0"/>
            </dependentAssembly>
        </assemblyBinding>
    </runtime>
    <oracle.manageddataaccess.client>
        <version number="*">
            <dataSources>
                <dataSource alias="mytest" descriptor="(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=test))) "/>
            </dataSources>
        </version>
    </oracle.manageddataaccess.client>
    <system.data>
        <DbProviderFactories>
            <remove invariant="Oracle.ManagedDataAccess.Client"/>
            <add name="ODP.NET, Managed Driver" invariant="Oracle.ManagedDataAccess.Client" description="Oracle Data Provider for .NET, Managed Driver" type="Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess, Version=4.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342"/>
        </DbProviderFactories>
    </system.data>
</configuration>

```
