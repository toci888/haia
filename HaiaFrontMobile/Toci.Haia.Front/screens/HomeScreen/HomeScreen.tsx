import React from "react";
import { useEffect, useState } from "react";
import { appSelector } from "../../store/Store";
import { useRoute } from "@react-navigation/native";
import { Post } from "../../domains/models/Post";
import { apiFetchSuggestedPostsRequest } from "../../domains/api/posts/getSuggestedPostsRequest";
import { View, StyleSheet, Text } from "react-native";
import PostCard from "./PostCard/PostCard";

const HomeScreen = () => {
  const { user: { currentUser } } = appSelector(s => s);
  const route = useRoute();

  const [Posts, setPosts] = useState<Post[]>([]);

  const fetchSuggestedPosts = async () => {
    const posts: Post[] | undefined = await apiFetchSuggestedPostsRequest(currentUser!.id);
    if(posts) {
      setPosts(posts);
    }
  };

  const handleRefreshPage = () => {
    console.log("handleRefreshPage")
    fetchSuggestedPosts();
  }

  useEffect(() => {
    fetchSuggestedPosts();
  }, [route]);

  return(
    <View style={styles.container}>
      <View style={{ flexDirection: 'row' }}>
        {currentUser && (
          <Text>Witaj {currentUser.login}</Text>
        )}
        <View>
          <Text style={styles.refreshButton} onPress={handleRefreshPage}>Refresh page</Text>
        </View>
      </View>
      <View>
        {Posts.length > 0 && 
          Posts.map((post: Post) => (
            <PostCard key={post.id} post={post} />
          ))
        }
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
    padding: 10,
  },
  refreshButton: {
    backgroundColor: '#28a745',
    padding: 10,
    borderRadius: 10,
    margin: 10,
    width: 90,
  },
});

export default HomeScreen;