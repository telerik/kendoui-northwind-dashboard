#!/usr/bin/env bash
echo "Stage1 Find Updates"
LATEST_RELEASE=$(curl -s https://api.github.com/repos/telerik/kendo-ui-core/releases | grep tag_name | head -n 1 |  cut -d '"' -f 4)
echo "Last release version is $LATEST_RELEASE"

function getCurrentVersion {
    for file in `find . -type f -name "*.cshtml"`  
    do
        CURRENT_VERSION=$(grep -hnr "kendo.cdn" $file | head -1 |cut -d '/' -f 4)
        if [ ! -z "$CURRENT_VERSION" ]
            then
                CURRENT_GLOBAL_VERSION=$CURRENT_VERSION
        fi
    done
}
    getCurrentVersion $file
    echo "Current version is $CURRENT_GLOBAL_VERSION"


for file in `find . -type f -name "*.cshtml"`  
do
    sed -i "s/$CURRENT_GLOBAL_VERSION/$LATEST_RELEASE/g" $file
done
for file in `find . -type f -name "*.csproj"`  
do
    sed -i "s/$CURRENT_GLOBAL_VERSION/$LATEST_RELEASE/g" $file
done
for file in `find . -type f -name "*.config"`  
do
    sed -i "s/$CURRENT_GLOBAL_VERSION/$LATEST_RELEASE/g" $file
done

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

    git diff
fi

