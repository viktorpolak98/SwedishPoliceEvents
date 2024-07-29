


const mapForLeaderboard = new Map();


function clearLeaderboardMap() {
    mapForLeaderboard.clear();
}


function addTypeToLeaderboardMap(type) {
    if (mapForLeaderboard.has(type)) {
        mapForLeaderboard.set(type, mapForLeaderboard.get(type) + 1);
        return;
    }

    mapForLeaderboard.set(type, 1);
}

function addItemsToLeaderboard() {
    
    const tableRef = document.getElementById(leaderboard);

    mapForLeaderboard.forEach(value, key => {
        const newRow = tableRef.insertRow(-1);
        
        const typeCell = newRow.insertCell(0);
        const countCell = newRow.insertCell(1);

        const typeText = document.createTextNode(key);
        typeCell.appendChild(typeText);

        const countText = document.createTextNode(value);
        countCell.appendChild(countText);
    });
}