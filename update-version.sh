layoutFile="html/index.html";
CURRENT_VERSION=$(grep -hnr "kendo.cdn" $layoutFile | head -1 |cut -d '/' -f 4)
echo "Current release version from $layoutFile is $CURRENT_VERSION"

LAST_RELEASE=$(curl -s https://api.github.com/repos/telerik/kendo-ui-core/releases | grep tag_name | head -n 1 |  cut -d '"' -f 4)
echo "Last release version is $LAST_RELEASE"

files=(angular/index.html aspnet-mvc/kendoui-northwind-dashboard/Views/Shared/_Layout.cshtml aspnet-mvc/kendoui-northwind-dashboard/kendoui-northwind-dashboard.csproj aspnet-mvc/kendoui-northwind-dashboard/packages.config html/about.html html/index.html html/products-orders.html html/team-efficiency.html)
for file in ${files[@]}
do
sed -i "s/$CURRENT_VERSION/$LAST_RELEASE/g" $file
done