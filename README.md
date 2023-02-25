# Github Action - Last Commit User Info

This action gets the last commit user info.
- Author full name
- Commit URL
- Author profile URL

## Usage

```yaml
# use default values
- uses: ClareAI/actions-last-commit-user-info@v1
```
or
```yaml
# specify values
- uses: ClareAI/actions-last-commit-user-info@v1
  id: last-commit-user-info
  with:
    auth-token: ${{ secrets.GITHUB_TOKEN }}
    repository: ${{ github.repository }}
    commit-sha: ${{ github.sha }}
```

then
```yaml
- name: Get the output
  run: |
    echo "Author full name: ${{ steps.last-commit-user-info.outputs.author-name }}"
    echo "Commit URL: ${{ steps.last-commit-user-info.outputs.commit-url }}"
    echo "Author profile URL: ${{ steps.last-commit-user-info.outputs.profile-url }}"
```
