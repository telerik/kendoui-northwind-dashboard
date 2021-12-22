#!/usr/bin/env bash
echo "Stage1 Find Updates"
LAST_RELEASE=$(curl -s https://api.github.com/repos/telerik/kendo-ui-core/releases | grep tag_name | head -n 1 |  cut -d '"' -f 4)
echo "Last release version is $LAST_RELEASE"

IFS="$IFS"
IFS=$'\n'
for file in `find . -type f -name "*.cshtml"`  
do
    echo "file = $file"
    ls $file
    CURRENT_VERSION=$(grep -hnr "kendo.cdn" $file | head -1 |cut -d '/' -f 4)
    echo "Current release version from $file is $CURRENT_VERSION"
    #  read line
    if [ -z "$CURRENT_VERSION" ]
        then
        echo "\$var is empty"
        else
        sed -i "s/$CURRENT_VERSION/$LAST_RELEASE/g" $file
              echo "\$var is NOT empty"
    fi
done
git diff

echo "Stage2 Commit the change"
reviewers="Dimitar-Goshev,MilenaCh,mparvanov"
echo $reviewers
BRANCH_NAME="update-dependencies"
PRs=$(gh pr list | grep "$BRANCH_NAME" || true)
echo "PRs are:"
echo $PRs
echo "Branch is:"
echo $BRANCH_NAME
if [ ! -z $PRs ]; then
    echo "Unmerged pr $BRANCH_NAME"
else
    git fetch origin
    git pull
    git checkout -b $BRANCH_NAME
    git config user.email "kendo-bot@progress.com"
    git config user.name "kendo-bot"
    git add . && git commit -m "chore: update dependencies"
    git pull
    git push -u origin $BRANCH_NAME
    gh pr create --base master --head $BRANCH_NAME --reviewer $reviewers \
    --title "Update dependencies $DATE" --body 'Please review and update dependencies'
fi

