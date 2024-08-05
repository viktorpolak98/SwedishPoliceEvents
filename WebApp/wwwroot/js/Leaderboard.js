


const mapForLeaderboard = new Map();


function clearLeaderboardMap() {
    mapForLeaderboard.clear();
}


export function addTypeToLeaderboardMap(type) {
    if (mapForLeaderboard.has(type)) {
        mapForLeaderboard.set(type, mapForLeaderboard.get(type) + 1);
        return;
    }

    mapForLeaderboard.set(type, 1);
}

export function addItemsToLeaderboard() {

    const tableRef = document.getElementById("leaderboard").getElementsByTagName("tbody")[0];

    mapForLeaderboard.forEach((value, key) => {
        
        const newRow = tableRef.insertRow(-1);
        
        const typeCell = newRow.insertCell(0);
        const countCell = newRow.insertCell(1);

        const typeText = document.createTextNode(key);
        typeCell.appendChild(typeText);

        const countText = document.createTextNode(value);
        countCell.appendChild(countText);
    });
}

export function clearLeaderboard() {
    clearLeaderboardMap();
    const tableRef = document.getElementById("leaderboard").getElementsByTagName("tbody")[0];

    while (tableRef.rows.length > 0) {
        tableRef.deleteRow(0);
    }
}

//Sorts leaderboard table. Since the contents of leaderboard is very small the time complexity won't make a difference.
//Therefor an algorithm with low space complexity is preferred

export function insertionSortTableDescending() {
    let table = document.getElementById("leaderboard");

    for (let i = 1; i < table.rows.length - 1; i++) {

        let j = i - 1; 
        while ((j >= 0) && (table.rows[i] > table.rows[j])) {
            table.rows.parentNode.insertBefore(table.rows[i], table.rows[j]);
            j--; 
        }
    }
}