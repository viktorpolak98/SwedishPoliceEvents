


export const mapForLeaderboard = new Map();


export function addTypeToLeaderboardMap(type) {
    if (mapForLeaderboard.has(type)) {
        mapForLeaderboard.set(type, mapForLeaderboard.get(type) + 1);
        return;
    }

    mapForLeaderboard.set(type, 1);
}