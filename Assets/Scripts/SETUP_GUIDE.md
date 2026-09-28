# RiftRandomizer Setup Guide

## New Configuration System

This project now uses a centralized configuration system. Follow these steps to set it up:

### 1. Create GameConfig ScriptableObject

1. In Unity Editor, right-click in the Project window (under `Assets/Resources` folder)
2. Select `Create > RiftRandomizer > Game Config`
3. Name it `GameConfig`
4. Configure the following settings:
   - **DataDragon Version**: Set to the latest League of Legends patch version (e.g., "13.17.1")
   - **Target Frame Rate**: Adjust as needed (default: 40)
   - **Scene Names**: Verify they match your scene names
   - **Randomization Constants**: Adjust max values as needed
   - **Connectivity Check URL**: Update if needed

### 2. Update Existing Scripts

All scripts have been updated to use the new systems:
- **SettingsManager**: Centralized PlayerPrefs management
- **DataDragonService**: Centralized API calls with caching
- **GameConfig**: Centralized configuration

### 3. Scene Setup

Make sure your scenes have:
- A `SettingsManager` instance (will be created automatically)
- A `DataDragonService` instance (will be created automatically)
- A `Notification` singleton (should already exist)

### 4. Breaking Changes

Some scripts have been refactored:
- `CoinFlip`: Now requires calling `FlipCoin()` method instead of running in Update()
- `ListManager`, `ListLockManager`, `ListLaneManager`: Now use `SetRandomSiblingIndex()` method instead of Update()
- All PlayerPrefs access now goes through `SettingsManager`

### 5. Testing

After setup:
1. Test API calls (champion selection, extreme bravery, etc.)
2. Verify settings persistence (lock status, lane status, etc.)
3. Check that notifications work correctly
4. Test scene navigation

## Migration Notes

- Old PlayerPrefs keys are still compatible
- API versions are now configurable via GameConfig
- All hardcoded URLs moved to GameConfig
- Magic numbers extracted to constants or config

