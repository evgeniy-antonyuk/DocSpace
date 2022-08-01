import React, { useState, useEffect } from "react";
import { inject, observer } from "mobx-react";
import { useTranslation } from "react-i18next";
import { CreateRoomDialog } from "../dialogs";

const CreateRoomEvent = ({
  visible,
  onClose,

  createRoom,
  createTag,
  addTagsToRoom,
  fetchTags,

  currrentFolderId,
  updateCurrentFolder,
}) => {
  const { t } = useTranslation([
    "CreateEditRoomDialog",
    "Common",
    "Settings",
    "Home",
  ]);
  const [fetchedTags, setFetchedTags] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  const onCreate = async (roomParams) => {
    console.log(roomParams);
    const createRoomParams = {
      roomType: roomParams.type,
      title: roomParams.title || "New room",
    };

    const tags = roomParams.tags.map((tag) => tag.name);
    const newTags = roomParams.tags.filter((t) => t.isNew).map((t) => t.name);
    console.log(tags, newTags);

    try {
      setIsLoading(true);

      const room = await createRoom(createRoomParams);
      console.log(room);
      for (let i = 0; i < newTags.length; i++) await createTag(newTags[i]);
      await addTagsToRoom(room.id, tags);
      await updateCurrentFolder(null, currrentFolderId);
    } catch (err) {
      console.log(err);
    } finally {
      setIsLoading(false);
      onClose();
    }
  };

  useEffect(async () => {
    let tags = await fetchTags();
    setFetchedTags(tags);
  }, []);

  return (
    <CreateRoomDialog
      t={t}
      visible={visible}
      onClose={onClose}
      onCreate={onCreate}
      fetchedTags={fetchedTags}
      isLoading={isLoading}
    />
  );
};

export default inject(
  ({ filesStore, tagsStore, filesActionsStore, selectedFolderStore }) => {
    const { createRoom, addTagsToRoom } = filesStore;
    const { createTag, fetchTags } = tagsStore;

    const { id: currrentFolderId } = selectedFolderStore;
    const { updateCurrentFolder } = filesActionsStore;

    return {
      createRoom,
      createTag,
      fetchTags,
      addTagsToRoom,
      currrentFolderId,
      updateCurrentFolder,
    };
  }
)(observer(CreateRoomEvent));
