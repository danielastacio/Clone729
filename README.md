# Clone 729
Clone 729 PC game

# github details

Branching strategy: 

1. dev - What we use for our daily work. 
2. main - Used as a staging area to create a new release. Code in main is merged in from the dev branch.
2. release - Periodic snapshots of the main branch that we will use as a versioned release (daily builds, alpha builds, beta builds, final release build, etc.)

General work flow for developing code: 

local code updates -> dev -> main -> release
